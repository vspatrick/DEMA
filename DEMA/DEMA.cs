// Patrick VIVES ©  2021. All rights reserved.
// Double exponential Moving Average.
// Version 1.0


using System;
using System.Drawing;
using TradingPlatform.BusinessLayer;

namespace DEMA
{

	public class DEMA : Indicator
    {
     
        #region Parameters
        [InputParameter("Period of Simple Moving Average", 0, 1, 999, 1, 1)]
        public int Period = 10;


        // Displays Input Parameter as dropdown list.
        [InputParameter("Sources prices for MA", 1, variants: new object[]{
            "Close", PriceType.Close,
            "Open", PriceType.Open,
            "High", PriceType.High,
            "Low", PriceType.Low,
            "Typical", PriceType.Typical,
            "Median", PriceType.Median,
            "Weighted", PriceType.Weighted
        })]
        public PriceType SourcePriceType = PriceType.Close;
        #endregion

     

        public DEMA()
            : base()
        {
            // Defines indicator's name and description.
            Name = "DEMA";
            Description = "Double Exponential mooving average";

            // Defines line on demand with particular parameters.
            AddLineSeries("DEMA", Color.CadetBlue, 1, LineStyle.Solid);

            // By default indicator will be applied on main window of the chart
            SeparateWindow = false;
        }

        /// <summary>
        /// This function will be called after creating an indicator as well as after its input params reset or chart (symbol or timeframe) updates.
        /// </summary>
        protected override void OnInit()
        {
            // Add your initialization code here
            ShortName = "DEMA (" + Period + ":" + SourcePriceType.ToString() + ")";

        }

   
        protected override void OnUpdate(UpdateArgs args)
        {
         
            if (Count <= Period) return;

            double lastEMA = 0.0;
            double lastEMA_of_EMA = 0.0;
            double weight = 2.0 / (1 + Period);

            for (int i = Count-1; i > 0; i--)
            {
                
                lastEMA = weight * GetPrice(SourcePriceType,i) + (1.0 - weight) * lastEMA;
                lastEMA_of_EMA = weight * lastEMA + (1.0 - weight) * lastEMA_of_EMA;
                SetValue(2.0 * lastEMA - lastEMA_of_EMA,0,i);
            }

            // Calculate very last Bar
            double EMA = weight * GetPrice(SourcePriceType, 0) + (1.0 - weight) * lastEMA;
            double EMA_of_EMA = weight * EMA + (1.0 - weight) * lastEMA_of_EMA;
            SetValue(2.0 * EMA - EMA_of_EMA, 0, 0);

        }
    }
}
