// Regression: scope-extractor froze the bindings array for `User`
// (because Program.cs locally declares `class User`). Then
// populateCsharpNamespaceSiblings's `using Collision.Models;` tried
// to push the cross-file `User` into the SAME frozen array, throwing
// "Cannot add property N, object is not extensible" and abandoning
// scope resolution for the whole repo.
using Collision.Models;

namespace Collision.App
{
    // Local class with the same simple name as the cross-file sibling —
    // forces scope-extractor to pre-populate `User` in the Module scope's
    // bindings (frozen). The populator then injects another binding for
    // the same simple name via the `using Collision.Models;` directive.
    public class User
    {
        public string GetName() { return "app"; }
    }

    public class Program
    {
        public void Run()
        {
            // Local takes precedence (origin:local shadows origin:namespace);
            // the test only cares that the analysis doesn't blow up.
            var u = new User();
            u.GetName();
        }
    }
}
