all:	
	g++ chat_server.cpp /usr/local/stow/boost/boost_1_53_0/lib/libboost_*.a -I/usr/local/stow/boost/boost_1_53_0/include/ dependency_graph.cpp -o server -lpthread

server:	server.cpp
	g++ chat_server.cpp /usr/local/stow/boost/boost_1_53_0/lib/libboost_*.a -I/usr/local/stow/boost/boost_1_53_0/include/ -o server

rs: server
	./server 3000
	
clean:
	rm server client *.ss
