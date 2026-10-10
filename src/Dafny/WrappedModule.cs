// Dafny program WrappedModule.dfy compiled into C#
// To recompile, you will need the libraries
//     System.Runtime.Numerics.dll System.Collections.Immutable.dll
// but the 'dotnet' tool in .NET should pick those up automatically.
// Optionally, you may want to include compiler switches like
//     /debug /nowarn:162,164,168,183,219,436,1717,1718

using System;
using System.Numerics;
using System.Collections;
[assembly: DafnyAssembly.DafnySourceAttribute(@"// dafny 4.11.0.0
// Command-line arguments: translate cs WrappedModule.dfy --allow-warnings
// WrappedModule.dfy


module WrappedDafny {
  function WrappedFilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>
    decreases root, obs
  {
    FilterBy(root, obs)
  }

  import opened Filtering
}

module Filtering {
  function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>
    ensures forall i: int {:trigger FilterBy(root, obs)[i]} :: 0 <= i < |FilterBy(root, obs)| ==> FilterBy(root, obs)[i].getTaxon().isSubTaxon(root)
    ensures forall i: int {:trigger obs[i]} :: 0 <= i < |obs| ==> obs[i].getTaxon().isSubTaxon(root) ==> obs[i] in FilterBy(root, obs)
    decreases root, obs
  {
    if |obs| == 0 then
      []
    else if obs[0].getTaxon().isSubTaxon(root) then
      [obs[0]] + FilterBy(root, obs[1..])
    else
      FilterBy(root, obs[1..])
  }

  class {:extern} Taxon {
    function {:extern} isSubTaxon(ancestor: Taxon): bool
      decreases ancestor
  }

  class {:extern} Observation {
    function {:extern} getTaxon(): Taxon
  }
}
")]

namespace Dafny {
  internal class ArrayHelpers {
    public static T[] InitNewArray1<T>(T z, BigInteger size0) {
      int s0 = (int)size0;
      T[] a = new T[s0];
      for (int i0 = 0; i0 < s0; i0++) {
        a[i0] = z;
      }
      return a;
    }
  }
} // end of namespace Dafny
internal static class FuncExtensions {
  public static Func<UResult> DowncastClone<TResult, UResult>(this Func<TResult> F, Func<TResult, UResult> ResConv) {
    return () => ResConv(F());
  }
  public static Func<U, UResult> DowncastClone<T, TResult, U, UResult>(this Func<T, TResult> F, Func<U, T> ArgConv, Func<TResult, UResult> ResConv) {
    return arg => ResConv(F(ArgConv(arg)));
  }
  public static Func<U1, U2, UResult> DowncastClone<T1, T2, TResult, U1, U2, UResult>(this Func<T1, T2, TResult> F, Func<U1, T1> ArgConv1, Func<U2, T2> ArgConv2, Func<TResult, UResult> ResConv) {
    return (arg1, arg2) => ResConv(F(ArgConv1(arg1), ArgConv2(arg2)));
  }
}
// end of class FuncExtensions
namespace Filtering {

  public partial class __default {
    public static Dafny.ISequence<Filtering.Observation> FilterBy(Filtering.Taxon root, Dafny.ISequence<Filtering.Observation> obs)
    {
      Dafny.ISequence<Filtering.Observation> _0___accumulator = Dafny.Sequence<Filtering.Observation>.FromElements();
    TAIL_CALL_START: ;
      if ((new BigInteger((obs).Count)).Sign == 0) {
        return Dafny.Sequence<Filtering.Observation>.Concat(_0___accumulator, Dafny.Sequence<Filtering.Observation>.FromElements());
      } else if ((((obs).Select(BigInteger.Zero)).getTaxon()).isSubTaxon(root)) {
        _0___accumulator = Dafny.Sequence<Filtering.Observation>.Concat(_0___accumulator, Dafny.Sequence<Filtering.Observation>.FromElements((obs).Select(BigInteger.Zero)));
        Filtering.Taxon _in0 = root;
        Dafny.ISequence<Filtering.Observation> _in1 = (obs).Drop(BigInteger.One);
        root = _in0;
        obs = _in1;
        goto TAIL_CALL_START;
      } else {
        Filtering.Taxon _in2 = root;
        Dafny.ISequence<Filtering.Observation> _in3 = (obs).Drop(BigInteger.One);
        root = _in2;
        obs = _in3;
        goto TAIL_CALL_START;
      }
    }
  }


} // end of namespace Filtering
namespace WrappedDafny {

  public partial class __default {
    public static Dafny.ISequence<Filtering.Observation> WrappedFilterBy(Filtering.Taxon root, Dafny.ISequence<Filtering.Observation> obs)
    {
      return Filtering.__default.FilterBy(root, obs);
    }
  }
} // end of namespace WrappedDafny
namespace _module {

} // end of namespace _module
