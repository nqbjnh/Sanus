using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace Sanus.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Tab1 : ContentPage
	{
	    private int _HeightRequest = 250;
	    private int _StepCount = 153237;
		public Tab1 ()
		{
			InitializeComponent ();

            /*Device.StartTimer(TimeSpan.FromMilliseconds(1f/60), () =>
            {
                chartView.InvalidateSurface();
                return true;
            });*/
		}

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
            //Microcharts.Chart c = new
	        chartView.HeightRequest = _HeightRequest;
	        chartView.Chart = new DonutChart()
	        {
                Margin = _HeightRequest/3,
	            BackgroundColor = SKColor.Empty,
	            HoleRadius = 0.9f, //90%
	            Entries = new[]
	            {
	                new Entry(100)
	                {
	                    Color = SKColor.Parse("#23b8f9"),

	                },
	                new Entry(200)
	                {
	                    Color = SKColor.Parse("#ff0000"),

	                },

	            }
	        };

	        chartView2.Chart = new DonutChart()
	        {

	            BackgroundColor = SKColor.Empty,
                MinValue = 10,
	            MaxValue = 400,
                
	            Entries = new[]
	            {
	                new Entry(300)
	                {
	                    Color = SKColor.Parse("#ffffff"),

	                },
	                new Entry(100)
	                {
	                    Color = SKColor.Parse("#ff0000"),

	                },

	            }
	        };
        }

        private void ChartView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
	    {
	        SKImageInfo info = e.Info;
	        SKSurface surface = e.Surface;
	        SKCanvas canvas = surface.Canvas;
            
	        var radius = (Math.Min(info.Width, info.Height) - (2 * _HeightRequest / 3)) / 2;

            //canvas.Clear();
            SKBlurStyle blur = SKBlurStyle.Solid;
	        SKPaint circle1 = new SKPaint
	        {
	            Color = Color.FromRgba(1, 106, 187, 255).ToSKColor(),
	           
	            MaskFilter = SKMaskFilter.CreateBlur(blur, 1)
	        };
	        SKPaint circle2 = new SKPaint
	        {
	            Color = Color.White.ToSKColor(),
	            StrokeWidth = 5,
	            IsStroke = true,
	            MaskFilter = SKMaskFilter.CreateBlur(blur, 1)
	        };

	      
	        var stepCount = new SKPaint
	        {
	            TextSize = (float) ((float)_HeightRequest/2.5),
	            FakeBoldText = true,
	            IsAntialias = true,
	            Color = Color.White.ToSKColor(),
	            IsStroke = false,
	            TextAlign = SKTextAlign.Center,

	        };

	        var stepText = new SKPaint
	        {
	            TextSize = (float)_HeightRequest / 5,
	            FakeBoldText = true,
	            IsAntialias = true,
	            Color = Color.White.ToSKColor(),
	            IsStroke = false,
	            TextAlign = SKTextAlign.Center,

	        };
            var path = new SKPath()
            {
                Convexity =  SKPathConvexity.Concave,
                FillType = SKPathFillType.EvenOdd
            };


	        //canvas.DrawCircle(info.Width / 2, info.Height / 2, _HeightRequest - 50, circle1);
	       // canvas.Save();
	        
            //var extraRadius = DateTime.Now.Second + DateTime.Now.Millisecond/1000f;
	        canvas.DrawCircle(info.Width / 2, info.Height / 2, (float)radius + 10, circle2);
            //canvas.Restore();


            canvas.DrawText(_StepCount.ToString(), info.Width / 2, info.Height / 2 + 10, stepCount);
	        canvas.DrawText("BƯỚC", info.Width / 2, info.Height / 2 + 90, stepText);
	    }

	    private void ChartView_OnPaintSurface2(object sender, SKPaintSurfaceEventArgs e)
	    {
	        SKImageInfo info = e.Info;
	        SKSurface surface = e.Surface;
	        SKCanvas canvas = surface.Canvas;

	     
            //canvas.Clear();
            SKBlurStyle blur = SKBlurStyle.Solid;
	        SKPaint paint = new SKPaint
	        {
	            Color = Color.FromRgba(35, 184, 249, 255).ToSKColor(),
                MaskFilter = SKMaskFilter.CreateBlur(blur, 1)
	        };
	        SKPaint paint2 = new SKPaint
	        {
	            Color = Color.White.ToSKColor(),
	            StrokeWidth = 5,
	            IsStroke = true,
	            MaskFilter = SKMaskFilter.CreateBlur(blur, 1)
	        };

	        SKPath path = new SKPath();

	        var skPaint = new SKPaint
	        {
	            TextSize = 60.0f,
	            FakeBoldText = true,
	            IsAntialias = true,
	            Color = Color.White.ToSKColor(),
	            IsStroke = false,
	            TextAlign = SKTextAlign.Center,

	        };

	        var buoc = new SKPaint
	        {
	            TextSize = 20.0f,
	            FakeBoldText = true,
	            IsAntialias = true,
	            Color = Color.White.ToSKColor(),
	            IsStroke = false,
	            TextAlign = SKTextAlign.Center,

	        };

	        canvas.DrawCircle(info.Width / 2, info.Height / 2, 70, paint);
	        //canvas.DrawCircle(info.Width / 2, info.Height / 2, 15, paint2);
	        canvas.DrawText("80%", info.Width / 2, info.Height / 2 + 10, skPaint);
	        canvas.DrawText("mục tiêu", info.Width / 2, info.Height / 2 + 45, buoc);
	    }
    }
}