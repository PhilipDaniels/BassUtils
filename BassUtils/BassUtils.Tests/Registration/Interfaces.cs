using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BassUtils.Tests.Registration
{
    public interface PubInterfaceWithNoImplementations { }
    public interface PubInterfaceWithTwoImplementations { }
    interface PrivInterface { }

    public class FirstImpl : PubInterfaceWithTwoImplementations { }
    public abstract class SecondImpl : PubInterfaceWithTwoImplementations { }
    class ThirdImpl : PubInterfaceWithTwoImplementations { }                        // Private!
}
