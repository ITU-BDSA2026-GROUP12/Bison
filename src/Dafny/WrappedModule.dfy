//wraps the dafny filter so it can be called from our C# project
include "Filter.dfy"

module WrappedDafny {

    //Import filter function and external Taxon and Observation classes.
    import opened Filtering 
    import opened BisonModel

    //calls filter function defined in Filter.dfy
    function WrappedFilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>{
        FilterBy(root,obs)
    }

}