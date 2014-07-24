/*
 *This is the ported code from the dependency graph
 *
 */
#include <algorithm>
#include <iostream>
#include <vector>
#include <string>
#include <map>
#include "dependency_graph.h"
#include <set>
#include <sstream>

namespace depGraph
{
  std::map<std::string, std::vector<std::string> > DependeeCentricDictionary;
  std::map<std::string, std::vector<std::string> > DependentCentricDictionary;
  
  int size_var;
  /*
   *This is the constuctor for the dependency graph
   */
  dependency_graph::dependency_graph()
  {
    size_var = 0;
  }

  /*
   * the number of ordered paris in the dependency Graph
   */
  int  dependency_graph::size()
  {
    return size_var;
  }
  /*
   * Reports whether dependents is non empty
   */
  bool dependency_graph::HasDependents(std::string s)
  {
    std::vector<std::string> dependentDummyList;
    if (DependeeCentricDictionary.count(s) > 0)
    {
      dependentDummyList = DependeeCentricDictionary[s];
      return (dependentDummyList.size() > 0);
    }
    else 
    {
      return false;
    }
  }
  /*
   * Reports whether dependees is non empty
   */
  bool dependency_graph::HasDependees(std::string s)
  {
    std::vector<std::string> dependeeDummyList;
    if(DependentCentricDictionary.count(s) > 0)
    {
      dependeeDummyList = DependentCentricDictionary[s];
      return(dependeeDummyList.size() > 0);
    }
    else
    {
      return false;
    }
  }

  /*
   * Get values dependent on s
   */
  std::vector<std::string> dependency_graph::GetDependents(std::string s)
  {
    std::vector<std::string> dependentList;
    if(!HasDependents(s))
    {
      return dependentList;
    }
    return DependeeCentricDictionary[s];
  }

  /*
   * Get value that are dependees of s
   */
  std::vector<std::string> dependency_graph::GetDependees(std::string s)
  {
    std::vector<std::string> dependeeList;
    if(!HasDependees(s))
    {
      return dependeeList;
    }
    return DependentCentricDictionary[s];
  }

  /*
   * returns dependee centric map
   */
  std::map<std::string, std::vector<std::string> >  dependency_graph::getDependeeMap()
  {
    return DependeeCentricDictionary;
  }

  /* returns dependent centric map
   */
  std::map<std::string, std::vector<std::string> >  dependency_graph::getDependentMap()
  {
    return DependentCentricDictionary;
  }

  bool dependency_graph::add(std::string name, std::string contents)
  {
    std::vector<std::string> temp = GetDependents(name); //hold previous content
    std::vector<std::string> form_dependees;
    form_dependees = formula_dependees(contents);
    if(HasDependents(name))
    {
      ReplaceDependents(name, form_dependees);
      try
      {
        if(check_dep(name))
        {
          ReplaceDependents(name, temp);
          return false;
        }
      }
      catch (int e)
      {
          ReplaceDependents(name, temp);
          return false;
      }
    }
    else
    {
       for(std::vector<std::string>::size_type i = 0; i != form_dependees.size(); i++)
        {
          AddDependency(name, form_dependees[i]);
        }
      try
      {
        if(check_dep(name))
        {
          ReplaceDependents(name, temp);
          return false;
        }
      }
      catch (int e)
      {
          ReplaceDependents(name, temp);
          return false;
      }
    }
    return true;
  }
    
  /*
   * creates a depedency between s and t. where t is a dependee of s.
   */
  void dependency_graph::AddDependency(std::string s, std::string t)
  {
    std::vector<std::string> addDependentList;
    std::vector<std::string> addDependeeList;
    
    std::vector<std::string> addDependentValueList;
    std::vector<std::string> addDependeeValueList;

    //check to see if "s" is in the dependeeCentricDictionary
    if (DependeeCentricDictionary.count(s)>0)
    {
      std::vector<std::string> toAdd = DependeeCentricDictionary[s];
      addDependentList = toAdd;
      if (std::find(addDependentList.begin(), addDependentList.end(), t) != addDependentList.end())
      {
        return;
      }
      else
      {
        addDependentList.push_back(t);

        DependeeCentricDictionary[s] = addDependentList;

        if (DependentCentricDictionary.count(t) > 0 )
        {
            //get list associated with t
          std::vector<std::string> toAdd2 = DependentCentricDictionary[t];
          addDependeeList = toAdd2;

          addDependeeList.push_back(s);

          DependentCentricDictionary[t] = addDependeeList;

          size_var++;

          return;
        }
        else
        {
          addDependeeValueList.push_back(s);

          DependentCentricDictionary[t] = addDependeeValueList;

          size_var++;
          
          return;
        }
      }
    }
    else
    {
      addDependentValueList.push_back(t);

      DependeeCentricDictionary[s] = addDependentValueList;

      if (DependentCentricDictionary.count(t) > 0)
      {
        std::vector<std::string> toAdd3 = DependentCentricDictionary[t];
        addDependeeList = toAdd3;
        addDependeeList.push_back(s);

        DependentCentricDictionary[t] = addDependeeList;

        size_var++;
        return;
      }

      else
      {
        addDependeeValueList.push_back(s);

        DependentCentricDictionary[t] = addDependeeValueList;

        size_var++;
        return;
      }
    }
  }

  /*
   * Removes the dependency between s and t
   */
  void dependency_graph::RemoveDependency(std::string s, std::string t)
  {
    std::vector<std::string> sDependents;
    std::vector<std::string> tDependees;

    if(!DependeeCentricDictionary.count(s) > 0)
    {
      return;
    }
    else
    {
      sDependents = GetDependents(s);
      if(std::find(sDependents.begin(), sDependents.end(), t) != sDependents.end())
      {
        sDependents.erase(std::remove(sDependents.begin(), sDependents.end(), t), sDependents.end());
        DependeeCentricDictionary[s] = sDependents;

        tDependees = DependentCentricDictionary[t];
        tDependees.erase(std::remove(tDependees.begin(), tDependees.end(), s), tDependees.end());
        DependentCentricDictionary[t] = tDependees;

        size_var--;
        return;

      }
      else 
      {
        return;
      }
    }   
  }

  /*
   * replaces all dependents of s with newDependents
   */
  void dependency_graph::ReplaceDependents(std::string s, std::vector<std::string> newDependents)
  {
    std::vector<std::string> dependents = GetDependents(s);
    if(dependents.size() == 0)
    {
      return;
    }


    for(std::vector<std::string>::size_type i = 0; i != dependents.size(); i++)
    {
      RemoveDependency(s, dependents[i]);
    }

    for(std::vector<std::string>::size_type i = 0; i != newDependents.size(); i++)
    {
      AddDependency(s, newDependents[i]);
    }

  }

  /*Replaces all depndees of s with NewDepenees
   */
  void dependency_graph::ReplaceDependees(std::string s, std::vector<std::string> newDependees)
  {
    std::vector<std::string> dependees = GetDependees(s);
    if(dependees.size() == 0)
      {
        return;
      }

    
    for(std::vector<std::string>::size_type i = 0; i != dependees.size(); i++)
    {
      RemoveDependency(dependees[i], s);
    }

    for(std::vector<std::string>::size_type i = 0; i != newDependees.size(); i++)
    {
      AddDependency(newDependees[i], s);
    }

  }

  std::vector<std::string> dependency_graph::GetCellsToRecalculate(std::string name)
  {
    std::set<std::string> names;
    names.insert(name);
    return GetCellsToRecalculate(names);
  }

  std::vector<std::string> dependency_graph::GetCellsToRecalculate(std::set<std::string> names_set)
  {
    std::vector<std::string> changed;
    std::set<std::string> visited;
    std::set<std::string>::iterator it;
    for (it = names_set.begin(); it != names_set.end(); it++)
    {
      std::string name = *it;
      if(visited.count(name) > 0)
      {
      
      }
      else
      {
        visit(name, name, visited, changed);
      }
    }
    return changed;
  }

  void dependency_graph::visit(std::string start, std::string name, std::set<std::string> &visited, std::vector<std::string> &changed)
  {
    visited.insert(name);
    std::vector<std::string> DirectDependents = GetDependents(name);
    for (std::vector<std::string>::size_type i = 0;  i != DirectDependents.size(); i++)
    {
      if(DirectDependents[i] == start)
      {
       //std::cout << "Circular Dependency!!!!" << std::endl;
       throw 20;
      }
      else if (!visited.count(DirectDependents[i]) > 0)
      {
        visit(start, DirectDependents[i], visited, changed);
      }
    }
    std::vector<std::string>::iterator push;
    push = changed.begin();
    changed.insert(push, name);
  }

  bool dependency_graph::check_dep(std::string to_check)
  {
    std::vector<std::string> all_deps = GetCellsToRecalculate(to_check);

    for(std::vector<std::string>::size_type i = 0; i != all_deps.size(); i++)
    {
      try
      {
        GetCellsToRecalculate(all_deps[i]);
      }
      catch (int e)
      {
        return true;
      }
    }
    return false;
  }

  std::vector<std::string> dependency_graph::formula_dependees(std::string formula)
  {
    std::vector<std::string> dependees;

    // Remove the \n from the string if necessary
    if (formula[0] == '=')
    {
      formula = formula.substr(1, formula.size());
    }

    // Get each element in the formula, and if it is a cell 
    // then add it to the dependees list
    std::stringstream ss;
    std::string temp;
    for (unsigned int i = 0; i < formula.length(); ++i)
    {
      if (formula[i] == '(' || formula[i] == ')' || formula[i] == '+' || formula[i] == '-'|| formula[i] == '*' || formula[i] == '/')
      {
        //  Write string stream into a string
        ss >> temp;

        // Check if first part of the string is a letter, if it does then
        // add it to the dependees list
        if (temp[0] >=65 && temp[0] <=90)
          dependees.push_back(temp);

        // Reset variables
        temp = "";
        ss.clear();
        continue;
      }
      // Add char to stringsteam
      ss << formula[i];
    }

    // Get the last element
    ss >> temp;

    // Check if first part of the string is a letter, if it does then
    // add it to the dependees list
    if (temp[0] >=65 && temp[0] <=90)
      dependees.push_back(temp);

    return dependees;
  }
}

