//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>23.04.2025 21:12:07</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace Console.ObjectTracking
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Xml.Linq;

    public class Program
    {
        private static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("1. Object Tracking mit IChangeTracking");
                Console.WriteLine("2. Object Tracking IRevertibleChangeTracking");
                Console.WriteLine("X. Beenden");

                Console.WriteLine("Wählen Sie einen Menüpunkt oder 'x' für beenden");
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.X)
                {
                    Environment.Exit(0);
                }
                else
                {
                    if (key == ConsoleKey.D1)
                    {
                        MenuPoint1();
                    }
                    else if (key == ConsoleKey.D2)
                    {
                        MenuPoint2();
                    }
                }
            }
            while (true);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            ViewItemTrackingV1 v1 = new ViewItemTrackingV1();
            v1.Id = Guid.NewGuid();
            v1.Name = "Gerhard";
            v1.Developer = "C#";
            v1.Gehalt = 5500.0F;
            v1.Status = true;

            Console.WriteLine($"Änderungsstatus: {v1.IsChanged}; vor AcceptChanges()");

            v1.AcceptChanges();

            Console.WriteLine($"Änderungsstatus: {v1.IsChanged}; nach AcceptChanges()");

            Console.WriteLine("Mit einer beliebigen Taste zum Menü.");
            Console.ReadKey();
        }

        private static void MenuPoint2()
        {
            Console.Clear();

            /* Neu erstellen */
            ViewItemTrackingV2 v2 = new ViewItemTrackingV2();
            v2.Id = Guid.NewGuid();
            v2.Name = "Gerhard";
            v2.Developer = "C#";
            v2.Gehalt = 5500.0F;
            v2.Status = true;
            v2.AcceptChanges();

            Console.WriteLine($"Änderungsstatus: {v2.IsChanged};");

            v2.Developer = "C#, WPF";
            Console.WriteLine($"Änderungsstatus: {v2.IsChanged};");
            Console.WriteLine($"Änderung: {v2.Developer};");
            Console.WriteLine();
            foreach (KeyValuePair<string,object> item in v2.OriginalValues)
            {
                Console.WriteLine($"Änderungen: {item.Key}-{item.Value};");
            }

            Console.WriteLine();
            Console.WriteLine("RejectChanges()");
            v2.RejectChanges();

            Console.WriteLine($"Änderungsstatus: {v2.IsChanged};");
            Console.WriteLine($"Nach RejectChanges(): {v2.Developer};");

            Console.WriteLine("Mit einer beliebigen Taste zum Menü.");
            Console.ReadKey();
        }
    }

    public class ViewItemTrackingV1 : IChangeTracking
    {
        #region Properties
        private Guid _Id;
        public Guid Id
        {
            get => this._Id;
            set
            {
                if (this._Id != value)
                {
                    this._Id = value;
                    this.IsChanged = true;
                }
            }
        }

        private string _Name;
        public string Name
        {
            get => this._Name;
            set
            {
                if (this._Name != value)
                {
                    this._Name = value;
                    this.IsChanged = true;
                }
            }
        }

        private string _Developer;
        public string Developer
        {
            get => this._Developer;
            set
            {
                if (this._Developer != value)
                {
                    this._Developer = value;
                    this.IsChanged = true;
                }
            }
        }

        private float _Gehalt;
        public float Gehalt
        {
            get => this._Gehalt;
            set
            {
                if (this._Gehalt != value)
                {
                    this._Gehalt = value;
                    this.IsChanged = true;
                }
            }
        }

        private bool _Status;
        public bool Status
        {
            get => this._Status;
            set
            {
                if (this._Status != value)
                {
                    this._Status = value;
                    this.IsChanged = true;
                }
            }
        }
        #endregion Properties

        public bool IsChanged { get; private set; }

        public void AcceptChanges() => IsChanged = false;
    }

    public class ViewItemTrackingV2 : TrackingBase, IRevertibleChangeTracking
    {
        #region Properties
        private Guid _Id;
        public Guid Id
        {
            get => this._Id;
            set
            {
                if (this._Id != value)
                {
                    this._Id = value;
                    base.AddOriginalValues(nameof(Id), this._Id);
                }
            }
        }

        private string _Name;
        public string Name
        {
            get => this._Name;
            set
            {
                if (this._Name != value)
                {
                    base.AddOriginalValues(nameof(Name), this._Name);
                    this._Name = value;
                }
            }
        }

        private string _Developer;
        public string Developer
        {
            get => this._Developer;
            set
            {
                if (this._Developer != value)
                {
                    base.AddOriginalValues(nameof(Developer), this._Developer);
                    this._Developer = value;
                }
            }
        }

        private float _Gehalt;
        public float Gehalt
        {
            get => this._Gehalt;
            set
            {
                if (this._Gehalt != value)
                {
                    base.AddOriginalValues(nameof(Gehalt), this._Gehalt);
                    this._Gehalt = value;
                }
            }
        }

        private bool _Status;
        public bool Status
        {
            get => this._Status;
            set
            {
                if (this._Status != value)
                {
                    base.AddOriginalValues(nameof(Status), this._Status);
                    this._Status = value;
                }
            }
        }
        #endregion Properties

        public void RejectChanges()
        {
            foreach (KeyValuePair<string,object> property in this.OriginalValues)
            {
                this.GetType().GetRuntimeProperty(property.Key).SetValue(this, property.Value);
            }

            this.AcceptChanges();
        }

        public void AcceptChanges()
        {
            base.ResetChanged();
        }
    }

    public abstract class TrackingBase
    {
        private readonly ConcurrentDictionary<string, object> originalValues = new ConcurrentDictionary<string, object>();

        public int CountChanges { get { return this.originalValues.Count(); } }

        public ConcurrentDictionary<string, object> OriginalValues { get { return this.originalValues; } }

        public bool IsChanged { get; private set; }

        public void AddOriginalValues(string key, object originalValue)
        {
            if (this.originalValues.ContainsKey(key) == false)
            {
                this.originalValues[key] = originalValue;
                this.IsChanged = true;
            }
        }

        public void ResetChanged()
        {
            this.originalValues.Clear();
            this.IsChanged = false;
        }
    }
}
