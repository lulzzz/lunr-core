﻿using System;
using System.Threading.Tasks;

namespace Lunr
{
    public abstract class Field
    {
        protected Field(string name, double boost = 1)
        {
            if (name == "") throw new InvalidOperationException("Can't create a field with an empty name.");
            if (name.IndexOf('/') != -1) throw new InvalidOperationException($"Can't create a field with a '/' character in its name \"{name}\".");

            Name = name;
            Boost = boost;
        }

        /// <summary>
        /// The name of the field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Boost applied to all terms within this field.
        /// </summary>
        public double Boost { get; }

        public abstract object ExtractValue(Document doc);
    }

    /// <summary>
    /// Represents an index field.
    /// </summary>
    public class Field<T> : Field
    {
        public Field(string name, double boost = 1, Func<Document, Task<T>>? extractor = null) : base(name, boost)
            => Extractor = extractor ?? new Func<Document, Task<T>>(doc => Task.FromResult((T)doc[name]));

        /// <summary>
        /// Function to extract a field from a document.
        /// </summary>
        public Func<Document, Task<T>> Extractor { get; }

        public override object ExtractValue(Document doc)
            => Extractor(doc);
    }
}
