﻿using rsfa.contracts.daten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsfa.pfadbestimmung
{
   class PfadKandidat
   {
      private ImmutableStack<Strecke> strecken; 

      public PfadKandidat(Haltestelle starthaltestelle)
      {
         this.Starthaltestelle = starthaltestelle;
         this.strecken = ImmutableStack<Strecke>.Empty;
      }

      public Haltestelle Starthaltestelle { get; private set; }

      public IEnumerable<Strecke> Strecken { get { return this.strecken; } }

      public int StreckenCount { get { return this.strecken.Count; } }

      public Pfad ErstellePfad()
      {
         var pfad = new Pfad()
         {
            Starthaltestellenname = this.Starthaltestelle.Name,
            Strecken = GetStreckenArray()
         };

         return pfad;
      }

      private Strecke[] GetStreckenArray()
      {
          var index = this.strecken.Count;
          var streckenArray = new Strecke[index];
          foreach (var strecke in this.strecken)
          {
              streckenArray[--index] = strecke;
          }
          return streckenArray;
      }

      public PfadKandidat Clone()
      {
          return (PfadKandidat)this.MemberwiseClone();
      }

      internal void AddStrecke(Strecke strecke)
      {
          this.strecken = this.strecken.Push(strecke);
      }

      public string Zielhaltestellenname { get { return this.strecken.Top.Zielhaltestellenname; } }
   }
}
