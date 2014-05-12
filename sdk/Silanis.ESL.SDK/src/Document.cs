using System;
using System.Collections.Generic;
using Silanis.ESL.SDK.Internal.Conversion;

namespace Silanis.ESL.SDK
{
	public class Document
	{
		private List<Signature> signatures = new List<Signature>();
		private List<Field> fields = new List<Field> ();

		public string Name {
			get;
			set;
		}

		public string Id {
			get;
			set;
		}

		public string FileName {
			get;
			set;
		}

		public byte[] Content {
			get;
			set;
		}

		public int Index {
			get;
			set;
		}

		public bool Extract {
			get;
			set;
		}

        public string Description
        {
            get;
            set;
        }

        public List<Signature> Signatures
        {
            get
            {
                return signatures;
            }
        }       

        public List<Field> Fields
        {
            get
            {
                return fields;
            }
        }  
         
		public void AddSignatures (IList<Signature> signatures)
		{
			this.signatures.AddRange (signatures);
		}

		public void AddFields (IList<Field> fields)
		{
			this.fields.AddRange (fields);
		}
	}
}