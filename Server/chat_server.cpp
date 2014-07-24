//
// chat_server.cpp
// ~~~~~~~~~~~~~~~
//
// Copyright (c) 2003-2011 Christopher M. Kohlhoff (chris at kohlhoff dot com)
//
// Distributed under the Boost Software License, Version 1.0. (See accompanying
// file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
//

#include <algorithm>
#include <cstdlib>
#include <deque>
#include <iostream>
#include <sstream>
#include <fstream>
#include <list>
#include <set>
#include <map>
#include <stack>
#include <string>
#include <boost/bind.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/enable_shared_from_this.hpp>
#include <boost/asio.hpp>
#include <boost/filesystem.hpp>
#include "dependency_graph.h"
#include <boost/thread.hpp>

#define DEBUG 1

using boost::asio::ip::tcp;

//----------------------------------------------------------------------

typedef std::deque<std::string> chat_message_queue;
typedef std::vector<std::string> string_list;
typedef std::vector<std::string>* string_list_ptr;
typedef std::vector<std::map<std::string, std::string>* > map_list;
typedef std::vector<std::map<std::string, std::string>* >* map_list_ptr;
typedef std::map<std::string, std::string> string_map;
typedef std::map<std::string, std::string>* string_map_ptr;
typedef std::stack<std::string> string_stack;
typedef std::stack<std::string>* string_stack_ptr;

typedef depGraph::dependency_graph dg;
typedef depGraph::dependency_graph* dg_ptr;

//----------------------------------------------------------------------

bool contains(string_list &, std::string);
void get_all(const boost::filesystem::path&, const std::string&, std::vector<std::string>&);
string_list parse_string(std::string s);

//----------------------------------------------------------------------

/*
 * Participant - A helper class to be able to invoke delivery to all unique sockets in a room when an update occurs.
 *
 */
class chat_participant
{
public:
  virtual ~chat_participant() {}
  virtual void deliver(const std::string& msg) = 0;
};

typedef boost::shared_ptr<chat_participant> chat_participant_ptr;

//----------------------------------------------------------------------
/*
 * Room - Keeps track of participants to send UPDATEs when an update occurs in the spreadsheet.
 * 
 */
class chat_room
{
public:
	chat_room()
	{
		name = "";
		version = "";
		data = new string_map();
		graph = new dg();
		undo_stack = new string_stack();
	}

  // Add a participant (session) to the list of participants to update
  void join(chat_participant_ptr participant)
  {
    participants_.insert(participant);
    //std::for_each(recent_msgs_.begin(), recent_msgs_.end(), boost::bind(&chat_participant::deliver, participant, _1));
  }

  // Remove a participant (session) from the list of participants to update
  void leave(chat_participant_ptr participant)
  {
    participants_.erase(participant);
  }

  // When we have a message to send, loop through all the participants (sessions) in the room and update them
  void deliver(const std::string& msg)
  {
    recent_msgs_.push_back(msg);

    std::for_each(participants_.begin(), participants_.end(), boost::bind(&chat_participant::deliver, _1, boost::ref(msg)));
  }
  
  // Saves cell contents onto undo stack, adds the new values to the data map and saves the file
  void input(std::string cell, std::string contents)
  {
  	if (isValid(cell, contents))
  	{
  		if (DEBUG == 1)
  			std::cout << "Adding cell " << cell << ": \"" <<  contents << "\" to room " << name << "." << std::endl;
  		
  		updateUndo(cell);
  		
  		// Add the data to the map and dependency graph
  		(*data)[cell] = contents;
  		
  		// Save the data to disk
  		save();
  	}
  }
  
  //TODO TODO TODO
  std::string remove(std::string cell)
  {
  	updateUndo(cell);
  	
  	// Remove cell from map
  	data->erase(cell);
  	
  	// Set cell to empty in the dependency graph
  	graph->add(cell, "");
  	
  	return cell + "\e";
  }
  
  // Saves the current contents of a cell onto the undo stack
  // Run this before making a change to the cell
  void updateUndo(std::string cell)
  {
  	// Add old cell contents to undo stack
  		string_map::iterator it = data->find(cell);
      	
      	std::string last;
  		if (it != data->end())
  		{
  			last = it->first + "\e" + it->second;
  		}
  		else
  		{
  			last = cell + "\e";
  		}
  		undo_stack->push(last);
  }
  
  // Seperate load command that does not check for errors, update the undo stack
  // or save the file.
  bool load(std::string cell, std::string contents)
  {
  	(*data)[cell] = contents;
  	return true;
  }
  
  // Undoes the last command by taking the last changed cell from the stack and updates it to it's old value
  // Increments the version
  std::string undo()
  {
  	if (undo_stack->size() > 0)
  	{
  		std::string ret = undo_stack->top();
  		undo_stack->pop();
  		return ret;
  	}
  	else
  		return "ERROR";
  }
  
  bool isValid(std::string cell, std::string contents)
  {
  	// Check for circular dependencies
  	return graph->add(cell, contents);
  }
  
  void addToDG(std::string cell, std::string contents)
  {
  	// Add to graph regardless of result
  	graph->add(cell, contents);
  }
  
  std::string makeLine()
  {
    // Build an escape delimated string line of the cell data
    // of the form {version}[esc]{cell}[esc]{contents} ...
  	std::string line = version;
  	
  	if (data->size() > 0)
	{
		for (string_map::iterator it = data->begin(); it != data->end(); ++it)
		{
			line += "\e" + it->first + "\e" + it->second;
		}
	}
	
	return line;
  }
  
  // Returns true if a cell by the given name exists in the data map
  bool cellExists(std::string cell)
  {
	string_map::iterator it = data->find(cell);
	if (it != data->end())
	{
		return true;
	}
	
	return false;
  }
  
  // Write the data to the file
  // File format is {version}[esc]{cell}[esc]{contents}...
  bool save()
  {
  	if (DEBUG == 1)
  		std::cout << "Saving " << name << "." << std::endl;
  
  	// Open file with truncate option (deletes anything after what we write)
  	char* filename = (char*)name.c_str();
  	std::fstream file(filename, std::ofstream::out | std::ofstream::trunc);
  	
  	// Make the data and write it
	std::string line = makeLine();
	file << line << "\n";
	
	// Close the file
	file.close();
  	return true;
  }
  
  // Return true if there are no more participants in the room (session needs to leave *before* checking this)
  bool isEmpty()
  {
  	if (participants_.size() == 0)
  		return true;
  	return false;
  }
  
  void incrementVersion()
  {
	std::ostringstream ss;
	ss << std::atoi((char*)version.c_str()) + 1;
	version = ss.str();
  }

private:
  std::set<chat_participant_ptr> participants_; // list of participants to update when we have a message to send
  chat_message_queue recent_msgs_; // queue of messages to send to participants
  string_map_ptr data; // cell contents stored as a string in a map, mapped by the cell name
  dg_ptr graph;
  string_stack_ptr undo_stack;
  
  //TODO make getters and setters for name and version
public:
  std::string name; // filename
  std::string version; // current version of the open file
  
};

//----------------------------------------------------------------------

typedef std::map<std::string, chat_room*> room_map_list;
typedef std::map<std::string, chat_room*> *room_map_list_ptr;

//----------------------------------------------------------------------

/*
 * Session - All the main work is done here and is done by states.
 *           At the start, the session will setup the socket and enter "Password State"
 *
 * Password State: Listens for incoming password message.
 *     				Succeeds when a correct password has been entered. The session will send a list of files to the client and go to the Filename State.
 					Fails with incorrect password, sends an INVALID command and returns to the Password State.
 * Filename State: Listens for an OPEN or CREATE command.
 *                  Succeeds when there are no errors opening or creating a file. Joins the session to the new/existing room and continues to the Update State.
 * 					Fails if a file cannot be opened or created. Sends an ERROR and returns to the Filename State.
 * Update State:   Listens for incoming UPDATE commands from the client.
 *					Succeeds when a successful change is made. It will add the update to the delivery list and continue to the Update State again.
 *                  Fails when a change is unsuccessful. It will send an ERROR and return to the Update State
 *
 */
class chat_session
  : public chat_participant,
    public boost::enable_shared_from_this<chat_session>
{
public:
  chat_session(boost::asio::io_service& io_service, string_list_ptr filelist, room_map_list_ptr room_list)
    : socket_(io_service),
      filelist_(filelist),
      room_list_(room_list)
  {
  }

  tcp::socket& socket()
  {
    return socket_;
  }

  void start()
  {
    std::cout << "New connection." << std::endl;
    boost::asio::async_read_until(socket_, buffer, '\n', boost::bind(&chat_session::handle_read_password, shared_from_this(), boost::asio::placeholders::error));
  }
  
  //----- Password State -----//
  void read_password(const boost::system::error_code& error)
  {
  	if (!error)
    {
  		boost::asio::async_read_until(socket_, buffer, '\n', boost::bind(&chat_session::handle_read_password, shared_from_this(), boost::asio::placeholders::error));
  	}
  }

  void handle_read_password(const boost::system::error_code& error)
  {
    if (!error)
    {
    	std::string line = getLine();
    	string_list parts;
    	
    	parts = parse_string(line);
    		
    	if (parts[0] == "PASSWORD" && parts[1] == "orange")
    	{
    		// Generate a list of files
    		message = "FILELIST";
    		for (string_list::iterator it = filelist_->begin(); it != filelist_->end(); ++it)
    		{
    			message += "\e" + *it;
    		}
    		message += "\n";
    		
    		//message = "FILESLIST\efile.ss\eother.ss\eanother.ss\n";
    		boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::read_filename, shared_from_this(), boost::asio::placeholders::error));
    	}
    	else
    	{
    		message = "INVALID\n";
    		boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::read_password, shared_from_this(), boost::asio::placeholders::error));
    	}
    }
  }

  //----- Filename State -----//
  void read_filename(const boost::system::error_code& error)
  {
    if (!error)
    {
      // Wait for command from client
      boost::asio::async_read_until(socket_, buffer, '\n', boost::bind(&chat_session::handle_read_filename, shared_from_this(), boost::asio::placeholders::error));
    }
  }
  
  void handle_read_filename(const boost::system::error_code& error)
  {
  	if (!error)
    { 
      //Make a new file or open one and send data to client
      std::string line = getLine();
      string_list parts = parse_string(line);
      
      if (parts[0] == "OPEN")
      {
      	if (contains((*filelist_), parts[1]))
      	{
      		// if it does, add the user to the room and send the update to the client
      		// if it doesn't, make a new room, load the file and send the update to the client
      		
      		room_map_list::iterator it = room_list_->find(parts[1]);
      		
      		if (it != room_list_->end())
      		{
      			std::cout << "Client joined room " << it->first << "." << std::endl;
      			// Room found
      			room_ = it->second;
      			
      			// Get cell data from the room
      			line = room_->makeLine();
      			
      		}
      		else
      		{
      			// Room not found
      			//chat_room *room;
      			room_ = new chat_room();
      			room_->name = parts[1];
      			
      			// Load information from the file
      			char *filename = (char*)parts[1].c_str();
      			std::fstream file(filename);
      			getline(file, line, '\n');
      			
      			// Parse cell data and add it to the room data
      			parts = parse_string(line);
      			room_->version = parts[0];
      			
      			int i;
      			for (i = 1; i < parts.size() - 1; i += 2)
      			{
      				room_->load(parts[i], parts[i+1]);
      				if ((parts[i+1])[0] == '=')
      					room_->addToDG(parts[i], parts[i+1]);
      			}
      			
      			
      			// Add the room to the room_list_
      			(*room_list_)[room_->name] = room_;
      			
      			std::cout << "Client started room " << room_->name << "." << std::endl;
      		}
      		
      		room_->join(shared_from_this());
      		
      		// Open file and send information to client
		  	message = "UPDATE\e" + line + "\n";
		  	boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::handle_write_update, shared_from_this(), boost::asio::placeholders::error));
		}
		else
		{
			message = "ERROR\eFile does not exist.\n";
			boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::read_filename, shared_from_this(), boost::asio::placeholders::error));
		}
      }
      else if (parts[0] == "CREATE")
      {
      	if (contains((*filelist_), parts[1]))
      	{
      		message = "ERROR\eFile already exists.\n";
      		boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::read_filename, shared_from_this(), boost::asio::placeholders::error));
      	}
      	else
      	{
      		// Make a new file and room for the file
		  	room_ = new chat_room();
		  	room_->name = parts[1];
		  	room_->version = "0";		  	
		  	
		  	// Add the room to the room_list_
		  	(*room_list_)[room_->name] = room_;
		  	
		  	// Join the room
		  	room_->join(shared_from_this());
		  	
		  	// Save the room to disk (should be empty)
		  	room_->save();
		  	
		  	// Add the new spreadsheet to the list of files
		  	filelist_->push_back(room_->name);
		  	
		  	// Send the update (to version 0)
		  	message = "UPDATE\e0\n";
		  	boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::handle_write_update, shared_from_this(), boost::asio::placeholders::error));
		}
      }
      else
      {
      	message = "ERROR\eInvalid command: Expecting OPEN[esc]filename\\n or CREATE[esc]filename\\n.\n";
      	boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::read_filename, shared_from_this(), boost::asio::placeholders::error));
      }
    }
  }

  //----- Update State -----//
  void handle_write_update(const boost::system::error_code& error)
  {
  	if (!error)
    {
    	// Begin listening for updates from the client
    	boost::asio::async_read_until(socket_, buffer, '\n', boost::bind(&chat_session::handle_read_update, shared_from_this(), boost::asio::placeholders::error));
    }
    else
    {
    	disconnect();
    }
  }
  
  void handle_read_update(const boost::system::error_code& error)
  {
  	if (!error)
    {
    	std::string line = getLine();
    	string_list parts = parse_string(line);
    	if (DEBUG == 1)
    		std::cout << line << std::endl;
    	if (parts[0] == "ENTER")
    	{
    		
    		// Check version
    		if (parts[1] != room_->version)
    		{
    			message = "SYNC\e" + room_->makeLine() + "\n";
    			boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::handle_write_update, shared_from_this(), boost::asio::placeholders::error));
    		}
    		else
    		{
    			/*
				//TODO Check if cell is being removed
				// Remove cell from dependency graph and map
				// DO NOT use the input command (it will add it to the map)
				if (parts[3] == "")
				{
					
				}
				*/
				
				// Check if update is valid
				
				if ((parts[3])[0] == '=' && !room_->isValid(parts[2], parts[3]))
				{
					message = "ERROR\eCircular dependency.\n";
					boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::handle_write_update, shared_from_this(), boost::asio::placeholders::error));
				}
				else
				{
					// Add update to room spreadsheet data
					// Increment version
					room_->incrementVersion();
					
					// Input cell changes
					room_->input(parts[2], parts[3]);
					
					// Add the line to the room queue of updates
					std::string line = "UPDATE\e" + room_->version + "\e" + parts[2] + "\e" + parts[3] + "\n";
					room_->deliver(line);
					parts = parse_string(line);
					
					int i;
					std::cout << "Sending Update: ";
					for (i = 0; i < parts.size(); i++)
					{
						std::cout << parts[i] << " ";
					}
					std::cout << std::endl;
					
					// Continue listening for messages from the client
					boost::asio::async_read_until(socket_, buffer, '\n', boost::bind(&chat_session::handle_read_update, shared_from_this(), boost::asio::placeholders::error));
				}
			}
			
		}
		else if (parts[0] == "SAVE")
		{
			// Check version number, if correct, save the file
			if (room_->version == parts[1])
			{
				room_->save();
				message = "SAVED\n";
    			boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::handle_write_update, shared_from_this(), boost::asio::placeholders::error));
			}
		}
		else if (parts[0] == "UNDO")
		{
			if (parts[1] != room_->version)
    		{
    			message = "SYNC\e" + room_->makeLine() + "\n";
    			boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::handle_write_update, shared_from_this(), boost::asio::placeholders::error));
    		}
    		else
    		{
    			// Get undo line and parse
				std::string in = room_->undo();
				
				if (in == "ERROR")
				{
					message = "ERROR\eNothing to undo.\n";
					boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::handle_write_update, shared_from_this(), boost::asio::placeholders::error));
				}
				else
				{
					string_list parts = parse_string(in);
			
					// Increment version
					room_->incrementVersion();
				
					// Make changes to spreadsheet
					room_->load(parts[0], parts[1]);
					room_->save();
					
					// Deliver update to all clients
					room_->deliver("UPDATE\e" + room_->version + "\e" + in + "\n");
					
					// Continue listening for messages from the client
					boost::asio::async_read_until(socket_, buffer, '\n', boost::bind(&chat_session::handle_read_update, shared_from_this(), boost::asio::placeholders::error));
				}
				
			}
		}
		else if (parts[0] == "RESYNC")
		{
			message = "SYNC\e" + room_->makeLine() + "\n";
			boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::handle_write_update, shared_from_this(), boost::asio::placeholders::error));
		}
		else if (parts[0] == "DISCONNECT")
		{
			// Disconnect cleanly
			disconnect();
		}
		else
		{
			message = "ERROR\eInvalid command.\n";
			boost::asio::async_write(socket_, boost::asio::buffer(message), boost::bind(&chat_session::handle_write_update, shared_from_this(), boost::asio::placeholders::error));
		}
    }
    else
    {
      disconnect();
    }
  }

  // Deliver message from room
  void deliver(const std::string& msg)
  {
    bool write_in_progress = !write_msgs_.empty();
    write_msgs_.push_back(msg);
    if (!write_in_progress)
    {
      boost::asio::async_write(socket_, boost::asio::buffer(write_msgs_.front().data(), write_msgs_.front().length()), boost::bind(&chat_session::handle_write, shared_from_this(), boost::asio::placeholders::error));
    }
  }
  
  void handle_write(const boost::system::error_code& error)
  {
  	if (!error)
    {
    	// Begin listening for updates from the client
    	write_msgs_.pop_front();
    	if (!write_msgs_.empty())
    	{
    		boost::asio::async_write(socket_, boost::asio::buffer(write_msgs_.front().data(), write_msgs_.front().length()), boost::bind(&chat_session::handle_write, shared_from_this(), boost::asio::placeholders::error));
    	}
    }
    else
    {
    	disconnect();
    }
  }
  
  void disconnect()
  {
  	std::cout << "Client left room " << room_->name << "." << std::endl;
      room_->leave(shared_from_this());
      
      
      // Check if you're the last participant in the room, and if so, initiate a save and then delete the room from the room_list_ (and free memory)
      if (room_->isEmpty())
      {
      	std::cout << "Closing room " << room_->name << "." << std::endl;
      	room_->save();
      	room_list_->erase(room_->name);
      	
      	//TODO free memory (room_) (look into shared_ptrs?)
      }
      
      //TODO test to see how lenient asio is about disconnects, if it reacts too early we may need to implement a timeout that will reping the client a few times before
      // ending the session
  }
  
  // Grab the next line from the read buffer
  std::string getLine()
  {
  		std::istream is(&buffer);
		std::string line;
		std::getline(is, line);
		return line;
  }

private:
  // Unique socket
  tcp::socket socket_;
  
  // Room data
  chat_room *room_; // Current room we've joined (for easy access)
  room_map_list_ptr room_list_; // List of rooms from the server so we can join or create one
  
  // Message data
  chat_message_queue write_msgs_; // Message queue to be sent to the client
  std::string message; // Message to be sent to the client
  boost::asio::streambuf buffer; // Buffer used to receive incoming messages from client
  
  // File data
  string_list_ptr filelist_; // List of file names from the server so we know what is available to open/create
};

//----------------------------------------------------------------------

typedef boost::shared_ptr<chat_session> chat_session_ptr;

void *readLine(void*)
	{
		std::string line = "";
		while (line != "exit")
		{
			std::cin >> line;
		}
		
		//TODO do any clean shut down stuff we might need to do..
		// save files, etc.
		
		exit(EXIT_SUCCESS);
	}


/*
 * Server - Sets up filelist and other data and asynchronously listens for incoming connections
 *          and gives each connection a new, unique session.
 *          Sessions are handled seperatly in the session class.
 *
 */
class chat_server
{
public:
	
  chat_server(boost::asio::io_service& io_service,
      const tcp::endpoint& endpoint)
    : io_service_(io_service),
      acceptor_(io_service, endpoint)
  {
  	// SETUP
  	std::cout << "Server is starting up...";
  	
  	// Get new thread to listen for commands from command line
  	pthread_t threads[1];
  	int rc = pthread_create(&threads[0], NULL, readLine, (void*)1);
  	
  	// Get files from directory
  	// TODO see if it can find a way to get a relative path from the executable
  	get_all(boost::filesystem::path("/home/mmourits/classes/cs3505/"), ".ss", filelist_);
  	
  	// Setup done, begin accepting connections.
    std::cout <<"done. Listening for connections." << std::endl;
    start_accept();
  }

  // Make a session and wait for a connection
  void start_accept()
  {
    chat_session_ptr new_session(new chat_session(io_service_, &filelist_, &room_list_));
    acceptor_.async_accept(new_session->socket(), boost::bind(&chat_server::handle_accept, this, new_session, boost::asio::placeholders::error));
  }

  // When someone connects, start their session then go back and make a new session for the next connection.
  void handle_accept(chat_session_ptr session, const boost::system::error_code& error)
  {
    if (!error)
    {
      session->start();
    }

    start_accept();
  }

private:
  // Networking
  boost::asio::io_service& io_service_; // socket information
  tcp::acceptor acceptor_; // server listener
  
  // Server side lists
  // These lists are maintainted by the server and passed as a pointer to each session for access
  chat_room room_; // dummy room
  room_map_list room_list_; // list of open/available rooms
  string_list filelist_; // list of available files
  //map_list sslist_; // list of open spreadsheet data
};

//----------------------------------------------------------------------

typedef boost::shared_ptr<chat_server> chat_server_ptr;
typedef std::list<chat_server_ptr> chat_server_list;

/*
 * Main - Gets port and starts the server.
 *
 */
int main(int argc, char* argv[])
{
  try
  {
    if (argc < 2)
    {
      std::cerr << "Usage: chat_server <port>\n";
      return 1;
    }

    boost::asio::io_service io_service;

    chat_server_list servers;
    tcp::endpoint endpoint(tcp::v4(), std::atoi(argv[1]));
    chat_server_ptr server(new chat_server(io_service, endpoint));
    servers.push_back(server);

    io_service.run();
  }
  catch (std::exception& e)
  {
    std::cerr << "Exception: " << e.what() << "\n";
  }

  return 0;
}

//----------------------------------------------------------------------

/*
 * Helper Functions
 *
 */
 
bool contains(string_list &in, std::string s)
{
	bool ret = false;
	
	for (string_list::iterator it = in.begin(); it != in.end(); ++it)
	{
		if (*it == s)
			ret = true;
	}
	
	return ret;
}

namespace fs = ::boost::filesystem;

// return the filenames of all files that have the specified extension
// in the specified directory and all subdirectories
void get_all(const fs::path& root, const std::string& ext, std::vector<std::string>& ret)
{  
  if (!fs::exists(root)) return;

  if (fs::is_directory(root))
  {
    fs::recursive_directory_iterator it(root);
    fs::recursive_directory_iterator endit;
    while(it != endit)
    {
      if (fs::is_regular_file(*it) and it->path().extension() == ext)
      {
        ret.push_back(it->path().filename().string());
      }
      ++it;
    }
  }
}

/*
 * Parse the string and returns a list of the input string
 * deliminated by the escape ascii char
 */

using namespace std;
 
vector<string> parse_string(string s)
{
  // Get the escape char into a string
  int esc = 27;
  char esc_char = static_cast<char>(esc);
  stringstream ss;
  string esc_string;
  ss << esc_char;
  ss >> esc_string;

  // Split the string based on the [esc] character
  size_t pos = 0; 
  string token;
  int count = 0;
  vector<string> stuff;

  while ((pos=s.find(esc_string)) != string::npos)
  {
    token = s.substr(0, pos);
    stuff.push_back(token);
    s.erase(0, pos + esc_string.length());
  }
  // Last item in the list
  stuff.push_back(s);

  return stuff;
}
