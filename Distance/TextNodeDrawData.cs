﻿using FFXIVClientStructs.FFXIV.Component.GUI;

namespace Distance;

//*****TODO: Subclass the nameplate stuff and add a new default for that version.
public struct TextNodeDrawData
{
	public bool Show;	//Nameplate only
	public short PositionX;
	public short PositionY;

	public ushort Width;	//Nameplate only
	public ushort Height;   //Nameplate only

	public float ScaleX;    //Nameplate only
	public float ScaleY;    //Nameplate only

	public bool UseDepth;	//Nameplate only

	public byte Alpha;      //Nameplate only

	public byte TextColorA;
	public byte TextColorR;
	public byte TextColorG;
	public byte TextColorB;

	public byte EdgeColorA;
	public byte EdgeColorR;
	public byte EdgeColorG;
	public byte EdgeColorB;

	public byte FontSize;
	public byte AlignmentFontType;
	public byte LineSpacing;
	public byte CharSpacing;

	public static readonly TextNodeDrawData Default = new()
	{
		Show = true,
		PositionX = 1,
		PositionY = 1,
		Width = AtkNodeHelpers.DefaultTextNodeWidth,
		Height = AtkNodeHelpers.DefaultTextNodeHeight,
		ScaleX = 1f,
		ScaleY = 1f,
		UseDepth = false,
		Alpha = 255,
		TextColorA = 255,
		TextColorR = 255,
		TextColorG = 255,
		TextColorB = 255,
		EdgeColorA = 255,
		EdgeColorR = 255,
		EdgeColorG = 255,
		EdgeColorB = 255,
		FontSize = 12,
		AlignmentFontType = (byte)AlignmentType.TopLeft,
		LineSpacing = 24,
		CharSpacing = 1
	};
}
