using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace ClassLibraryTest
{
    public partial class Interface : Component
    {
        public Interface()
        {
            InitializeComponent();
        }

        public Interface(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
