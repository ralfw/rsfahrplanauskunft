using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rsfa.pfadbestimmung
{
    class ImmutableStack<T> : IEnumerable<T>
    {
        private static readonly ImmutableStack<T> emptyStack = new ImmutableStack<T>(null, default(T));

        private ImmutableStack<T> previous;

        private T value;

        private Int32 count = 0;
             
        private ImmutableStack(ImmutableStack<T> previous, T value )
        {
            this.value = value;
            this.previous = previous;

            if (previous != null)
            {
                this.count = previous.count + 1;
            }
        }

        public static ImmutableStack<T> Empty { get { return emptyStack; } }

        public Int32 Count { get { return this.count; } }

        public T Top { get { return this.value; } }

        public ImmutableStack<T> Push(T value)
        {
            return new ImmutableStack<T>(this, value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            ImmutableStack<T> current = this;
            while (current.previous != null)
            {
                yield return current.value;
                current = current.previous;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
