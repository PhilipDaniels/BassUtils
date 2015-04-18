using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
//using BassUtils.Registration;
using NUnit.Framework;

namespace BassUtils.Tests.Registration
{
    /*
    [TestFixture]
    public class ExportedTypeViewTests
    {

        [Test]
        public void Ctor_ForNullAssembly_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ExportedTypeView(null));
            Assert.AreEqual("assembly", ex.ParamName);
        }

        [Test]
        public void Ctor_ForValidAssembly_SetsAssemblyProperty()
        {
            var asm = Assembly.GetExecutingAssembly();
            var etv = new ExportedTypeView(asm);
            Assert.AreSame(asm, etv.Assembly);
        }

        [Test]
        public void OmitsNonExportedInterfaces()
        {
            var asm = Assembly.GetExecutingAssembly();
            var etv = new ExportedTypeView(asm);
            var ift = typeof(PrivInterface);
            Assert.IsFalse(etv.Interfaces.ContainsKey(ift));
        }

        [Test]
        public void ContainsExportedInterfaces()
        {
            var asm = Assembly.GetExecutingAssembly();
            var etv = new ExportedTypeView(asm);
            var ift = typeof(PubInterfaceWithNoImplementations);
            Assert.IsTrue(etv.Interfaces.ContainsKey(ift));
            // This interface has no implementations.
            Assert.AreEqual(0, etv.Interfaces[ift].Count());
        }

        [Test]
        public void ImplementationsOfExportedInterfacesOnlyIncludePublicTypes()
        {
            var asm = Assembly.GetExecutingAssembly();
            var etv = new ExportedTypeView(asm);
            var ift = typeof(PubInterfaceWithTwoImplementations);
            Assert.IsTrue(etv.Interfaces.ContainsKey(ift));
            Assert.AreEqual(2, etv.Interfaces[ift].Count());
        }

        [Test]
        public void InterfacesProperty_WhenIncludeAbstractIsFalse_DoesNotReturnAnyAbstractImplementations()
        {
            var asm = Assembly.GetExecutingAssembly();
            var etv = new ExportedTypeView(asm);
            var ift = typeof(PubInterfaceWithTwoImplementations);
            etv.IncludeAbstract = false;
            Assert.IsTrue(etv.Interfaces.ContainsKey(ift));
            Assert.AreEqual(1, etv.Interfaces[ift].Count());
        }
    }
    */
}
