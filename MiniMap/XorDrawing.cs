using System;
using System.Runtime.InteropServices;

namespace MiniMap
{
	public static class XorDrawing
	{
		[DllImport("user32.dll", EntryPoint = "GetDC", CallingConvention = CallingConvention.StdCall)]
		private extern static IntPtr GetDC(IntPtr hwnd);

		[DllImport("user32.dll", EntryPoint = "ReleaseDC", CallingConvention = CallingConvention.StdCall)]
		private extern static void ReleaseDC(IntPtr intPtr, IntPtr hdc);

		[DllImport("gdi32.dll", EntryPoint = "SetROP2", CallingConvention = CallingConvention.StdCall)]
		private extern static int SetROP2(IntPtr hdc, int fnDrawMode);

		[DllImport("gdi32.dll", EntryPoint = "MoveToEx", CallingConvention = CallingConvention.StdCall)]
		public extern static bool MoveToEx(IntPtr hdc, int x, int y, IntPtr lpPoint);

		[DllImport("gdi32.dll", EntryPoint = "LineTo", CallingConvention = CallingConvention.StdCall)]
		public extern static bool LineTo(IntPtr hdc, int x, int y);

		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll", EntryPoint = "Rectangle", CallingConvention = CallingConvention.StdCall)]
		public static extern void Rectangle(IntPtr hdc, int x1, int y1, int x2, int y2);

		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		private static extern IntPtr GetStockObject(int brStyle);

		[DllImport("gdi32.dll", SetLastError = true)]
		static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		[DllImport("gdi32.dll", EntryPoint = "SelectObject")]
		public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);

		[DllImport("gdi32.dll")]
		static extern bool DeleteObject(IntPtr target);

		[DllImport("gdi32.dll")]
		static extern IntPtr CreatePen(PenStyle fnPenStyle, int nWidth, uint crColor);

		const int NULL_BRUSH = 5;

		public enum BinaryRasterOperations
		{
			R2_BLACK = 1,
			R2_NOTMERGEPEN = 2,
			R2_MASKNOTPEN = 3,
			R2_NOTCOPYPEN = 4,
			R2_MASKPENNOT = 5,
			R2_NOT = 6,
			R2_XORPEN = 7,
			R2_NOTMASKPEN = 8,
			R2_MASKPEN = 9,
			R2_NOTXORPEN = 10,
			R2_NOP = 11,
			R2_MERGENOTPEN = 12,
			R2_COPYPEN = 13,
			R2_MERGEPENNOT = 14,
			R2_MERGEPEN = 15,
			R2_WHITE = 16
		}

		private enum PenStyle : int
		{
			PS_SOLID = 0,       //The pen is solid.
			PS_DASH = 1,        //The pen is dashed.
			PS_DOT = 2,         //The pen is dotted.
			PS_DASHDOT = 3,     //The pen has alternating dashes and dots.
			PS_DASHDOTDOT = 4,  //The pen has alternating dashes and double dots.
			PS_NULL = 5,        //The pen is invisible.
			PS_INSIDEFRAME = 6, // Normally when the edge is drawn, it’s centred on the outer edge meaning that half the width of the pen is drawn
								// outside the shape’s edge, half is inside the shape’s edge. When PS_INSIDEFRAME is specified the edge is drawn 
								// completely inside the outer edge of the shape.
			PS_USERSTYLE = 7,
			PS_ALTERNATE = 8,
			PS_STYLE_MASK = 0x0000000F,

			PS_ENDCAP_ROUND = 0x00000000,
			PS_ENDCAP_SQUARE = 0x00000100,
			PS_ENDCAP_FLAT = 0x00000200,
			PS_ENDCAP_MASK = 0x00000F00,

			PS_JOIN_ROUND = 0x00000000,
			PS_JOIN_BEVEL = 0x00001000,
			PS_JOIN_MITER = 0x00002000,
			PS_JOIN_MASK = 0x0000F000,

			PS_COSMETIC = 0x00000000,
			PS_GEOMETRIC = 0x00010000,
			PS_TYPE_MASK = 0x000F0000
		};

		public static IntPtr BeginDraw(IntPtr hwnd, out int oldRop, out IntPtr oldpen)
		{
			IntPtr hdc = GetDC(hwnd);
			IntPtr pen = CreatePen(PenStyle.PS_DOT, 1, 0);

			oldpen = SelectObject(hdc, pen);
			oldRop = SetROP2(hdc, (int)BinaryRasterOperations.R2_XORPEN);
			SelectObject(hdc, GetStockObject(NULL_BRUSH));

			return hdc;
		}

		public static void FinishDraw(IntPtr hwnd, IntPtr hdc, int oldRop, IntPtr oldpen)
		{
			SetROP2(hdc, oldRop);
			DeleteObject(SelectObject(hdc, oldpen));	//delete new pen (switch to oldpen)
			ReleaseDC(hwnd, hdc);						// Release device context.
			//DeleteObject(hdc);						//DeleteObject(hdc);
		}
	}
}
