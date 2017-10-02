using System;
using System.Text;

namespace com.drew.metadata.jpeg
{
	/// <summary>
	/// The Jpeg component class
	/// </summary>
	[Serializable]

	public class JpegComponent 
	{
		private int componentId;
        public int ComponentId
        {
            get
            {
                return this.componentId;
            }
        }

		private int quantizationTableNumber;
        public int QuantizationTableNumber
        {
            get
            {
                return this.quantizationTableNumber;
            }
        }

        private int samplingFactorByte;
        /// <summary>
        /// Gets the Horizontal Sampling Factor
        /// </summary>
        /// <returns>the Horizontal Sampling Factor</returns>
        public int HorizontalSamplingFactor
        {
            get
            {
                return this.samplingFactorByte & 0x0F;
            }
        }
        /// <summary>
        /// Gets the Vertical Sampling Factor
        /// </summary>
        /// <returns>the Vertical Sampling Factor</returns>
        public int VerticalSamplingFactor
        {
            get
            {
                return (this.samplingFactorByte >> 4) & 0x0F;
            }
        }

		/// <summary>
		/// The constructor of the object
		/// </summary>
		/// <param name="aComponentId">the component id</param>
		/// <param name="aSamplingFactorByte">the sampling lcFactor byte</param>
		/// <param name="aQuantizationTableNumber">the quantization table number</param>
		public JpegComponent(
			int aComponentId,
			int aSamplingFactorByte,
			int aQuantizationTableNumber) : base()
		{
			this.componentId = aComponentId;
			this.samplingFactorByte = aSamplingFactorByte;
			this.quantizationTableNumber = aQuantizationTableNumber;
		}

		/// <summary>
		/// The component name
		/// </summary>
		/// <returns>The component name</returns>
		public string GetComponentName() 
		{
			switch (this.componentId) 
			{
				case 1 :
					return "Y";
				case 2 :
					return "Cb";
				case 3 :
					return "Cr";
				case 4 :
					return "I";
				case 5 :
					return "Q";
                default :
                    throw new MetadataException("Unsupported component id: " + this.componentId);
			}			
		}

        /// <summary>
        /// Gives a representation of the JpegComponent.
        /// </summary>
        /// <returns>The JpegComponent in a readable way</returns>
        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append(this.componentId).Append(',');
		    buff.Append(this.samplingFactorByte).Append(',');
		    buff.Append(this.quantizationTableNumber).Append(',');
            return buff.ToString();
        }
	}
}
