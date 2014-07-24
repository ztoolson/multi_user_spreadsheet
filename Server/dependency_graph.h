/*
 *this is the header for the ported dependency graph
 *
 */

#ifndef DEPENDENCY_GRAPH_H
#define DEPENDENCY_GRAPH_H

#include <vector>
#include <string>
#include <map>
#include <set>

namespace depGraph
{
  class dependency_graph
  {
  public:
    dependency_graph();
    int size();
    bool HasDependents(std::string s);
    bool HasDependees(std::string s);
    std::vector<std::string> GetDependents(std::string s);
    std::vector<std::string> GetDependees(std::string s);
    void AddDependency(std::string s, std::string t);
    void RemoveDependency(std::string s, std::string t);
    void ReplaceDependents(std::string s, std::vector<std::string> newDependents);
    void ReplaceDependees(std::string s, std::vector<std::string> newDependees);
    std::map<std::string, std::vector<std::string> > getDependeeMap();
    std::map<std::string, std::vector<std::string> > getDependentMap();
    std::vector<std::string> GetCellsToRecalculate(std::string name);
    std::vector<std::string> GetCellsToRecalculate(std::set<std::string> names); 
    bool check_dep(std::string to_check);
    bool add(std::string name, std::string contents);

  private:
    std::vector<std::string> formula_dependees(std::string formula);
    void visit(std::string start, std::string name, std::set<std::string> &visited, std::vector<std::string> &changed);
    std::map<std::string, std::vector<std::string> > DependeeCentricDictionary;
    std::map<std::string, std::vector<std::string> > DependentCentricDictionary;
    int size_var;
  };
}

#endif
