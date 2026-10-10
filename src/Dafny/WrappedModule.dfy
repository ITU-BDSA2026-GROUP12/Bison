include "Filter.dfy"

module WrappedDafny {

    //allows us to use code available in module Filtering
    import opened Filtering 
    import opened BisonModel
    //code here (methods etc.)

    function WrappedFilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>{
        FilterBy(root,obs)
    }

}