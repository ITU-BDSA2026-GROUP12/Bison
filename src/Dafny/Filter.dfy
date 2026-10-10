module {:extern "DafnyAdapters"} BisonModel {

    class {:extern "TaxonAdapter"} Taxon {
        function {:extern} isSubTaxon(ancestor: Taxon): bool
    }

    class {:extern "ObservationAdapter"} Observation {
        function {:extern} getTaxon(): Taxon
    }
}


module Filtering{
    import opened BisonModel
    function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>



// this ensures clause is here to make sure that the result of a sequence of observations is a sub taxon
// of the root taxon, its because we want to filter obersvation with their root taxon.
// the first ensure qualifies for all the observtions in resullt
    ensures
    forall i: int::
          0 <= i < |FilterBy(root, obs)| ==>
          FilterBy(root, obs)[i].getTaxon().isSubTaxon(root)

    // this ensure makes sure only those observations who are sub taxon of the root taxon is in the result.
    ensures
    forall i: int ::
        0 <= i < |obs| ==>
        obs[i].getTaxon().isSubTaxon(root) ==>
        obs[i] in FilterBy(root, obs)

            
{

    // so basically, we want to filter observations by their taxon.
    // we want only those observations which taxon is a sub taxon of the root taxon
    // what this does it first we check the length og the sequence(a list of observations)
    // if the length is 0 then we return an empty list
    // else we check if the first observation's taxon is a sub taxon of the root taxon
    // if it is then we add it to the result and call the function recursively and we add the rest of the observation to the result
    // if its not then we just skip the first observation and call the function again with the rest of the observations
    if |obs| == 0 then 
    []
    else 
    if obs[0].getTaxon().isSubTaxon(root) then
            
            [obs[0]] + FilterBy(root, obs[1..])
        else
            FilterBy(root, obs[1..])
}
}