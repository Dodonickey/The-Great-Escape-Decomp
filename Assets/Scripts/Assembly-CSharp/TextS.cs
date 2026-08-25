using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class TextS
{
	public static GenericArray<Font> m_fonts;

	public static Hashtable m_styles;

	private static Style m_style;

	public static GenericArray<TextC> m_components;

	public static int m_componentCount = 500;

	public static void Initialize(int _maxFontCount)
	{
		m_fonts = new GenericArray<Font>(_maxFontCount);
		m_styles = new Hashtable();
		m_components = new GenericArray<TextC>(m_componentCount);
		for (int i = 0; i < _maxFontCount; i++)
		{
			m_fonts.m_array[i] = new Font();
			m_fonts.m_array[i].characters = new Character[400];
			m_fonts.m_array[i].p_spriteSheet = null;
			m_fonts.m_array[i].name = string.Empty;
		}
		for (int j = 0; j < m_componentCount; j++)
		{
			m_components.m_array[j] = new TextC();
			m_components.m_array[j].componentType = ComponentType.Text;
			m_components.m_array[j].index = j;
			m_components.m_array[j].textAreaAlignX = 0f;
			m_components.m_array[j].textAreaAlignY = 1f;
			m_components.m_array[j].textAreaHeight = 100f;
			m_components.m_array[j].textAreaWidth = 200f;
			m_components.m_array[j].update = false;
			m_components.m_array[j].textHorizontalAlign = Align.Left;
			m_components.m_array[j].textVerticalAlign = Align.Top;
			m_components.m_array[j].contentTC = null;
		}
	}

	public static Style AddStyle(string _styleName, Font _font)
	{
		Style style = null;
		style = ((!m_styles.Contains(_styleName)) ? new Style() : (m_styles[_styleName] as Style));
		style.name = _styleName;
		style.p_font = _font;
		style.lineHeight = _font.lineHeight;
		style.baseline = _font.baseline;
		style.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		style.xScale = 1f;
		style.yScale = 1f;
		m_styles.Add(_styleName, style);
		return style;
	}

	public static void RemoveStyle(string _styleName)
	{
		m_styles.Remove(_styleName);
	}

	public static void SetStyle(string _styleName)
	{
		m_style = m_styles[_styleName] as Style;
	}

	public static Font AddFont(string _fontName, string _fontFolder, int _maxCharacters, int _textureWidth, int _textureHeight, float _globalScale, Camera _camera)
	{
		int num = m_fonts.AddItem();
		Font font = m_fonts.m_array[num];
		font.name = _fontName;
		font.p_spriteSheet = SpriteS.AddSpriteSheet(_maxCharacters, _camera, Resources.Load(_fontFolder + _fontName + "-material") as Material, _globalScale);
		TextAsset textAsset = Resources.Load(_fontFolder + _fontName + "-properties") as TextAsset;
		StringReader stringReader = new StringReader(textAsset.text);
		string text;
		while ((text = stringReader.ReadLine()) != null)
		{
			string[] array = text.Split(' ');
			if (array[0] == "common")
			{
				string[] array2 = array[1].Split('=');
				font.lineHeight = Convert.ToInt32(array2[1]);
				string[] array3 = array[2].Split('=');
				font.baseline = Convert.ToInt32(array3[1]);
				string[] array4 = array[3].Split('=');
				font.width = Convert.ToInt32(array4[1]);
				string[] array5 = array[4].Split('=');
				font.height = Convert.ToInt32(array5[1]);
			}
			else
			{
				if (!(array[0] == "char"))
				{
					continue;
				}
				string[] array6 = array[1].Split('=');
				int num2 = Convert.ToInt32(array6[1]);
				if (num2 <= 0)
				{
					continue;
				}
				font.characters[num2] = new Character();
				for (int i = 1; i < array.Length; i++)
				{
					array6 = array[i].Split('=');
					if (array6[0] == "x")
					{
						font.characters[num2].x = Convert.ToInt32(array6[1]);
					}
					else if (array6[0] == "y")
					{
						font.characters[num2].y = Convert.ToInt32(array6[1]);
					}
					else if (array6[0] == "width")
					{
						font.characters[num2].width = Convert.ToInt32(array6[1]);
					}
					else if (array6[0] == "height")
					{
						font.characters[num2].height = Convert.ToInt32(array6[1]);
					}
					else if (array6[0] == "xoffset")
					{
						font.characters[num2].xOffset = Convert.ToInt32(array6[1]);
					}
					else if (array6[0] == "yoffset")
					{
						font.characters[num2].yOffset = Convert.ToInt32(array6[1]);
					}
					else if (array6[0] == "xadvance")
					{
						font.characters[num2].xAdvance = Convert.ToInt32(array6[1]);
					}
				}
			}
		}
		Resources.UnloadAsset(textAsset);
		textAsset = null;
		return font;
	}

	public static Font GetFont(string _fontName, out int _index)
	{
		_index = -1;
		int aliveCount = m_fonts.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			Font font = m_fonts.m_array[m_fonts.m_aliveIndices[i]];
			if (font.name == _fontName)
			{
				_index = i;
				return font;
			}
		}
		return null;
	}

	public static void RemoveFont(string _fontName)
	{
		int _index = -1;
		Font font = GetFont(_fontName, out _index);
		if (_index > -1)
		{
			SpriteS.RemoveSpriteSheet(font.p_spriteSheet);
			m_fonts.RemoveItem(_index);
		}
	}

	public static TextC AddMultilineComponent(TransformC _tc, string _text, float _fontSize, float _textAreaWidth, float _textAreaHeight, Align _horizontalAlign, Align _verticalAlign)
	{
		return AddComponent(_tc, _text, _fontSize, true, true, 0.5f, 0.5f, _textAreaWidth, _textAreaHeight, _horizontalAlign, _verticalAlign, 0f, 0f, 0f, 0f, 0f, 0f);
	}

	public static TextC AddMultilineComponent(TransformC _tc, string _text, float _fontSize, float _textAreaWidth, float _textAreaHeight)
	{
		return AddComponent(_tc, _text, _fontSize, true, true, 0.5f, 0.5f, _textAreaWidth, _textAreaHeight, Align.Left, Align.Top, 0f, 0f, 0f, 0f, 0f, 0f);
	}

	public static TextC AddMultilineComponent(TransformC _tc, string _text, float _fontSize, bool _isDynamic, float _textAreaAlignX, float _textAreaAlignY, float _textAreaWidth, float _textAreaHeight, Align _textHorizontalAlign, Align _textVerticalAlign, float _marginLeft, float _marginRight, float _marginTop, float _marginBottom)
	{
		return AddComponent(_tc, _text, _fontSize, _isDynamic, true, _textAreaAlignX, _textAreaAlignY, _textAreaWidth, _textAreaHeight, _textHorizontalAlign, _textVerticalAlign, _marginLeft, _marginRight, _marginTop, _marginBottom, 0f, 0f);
	}

	public static TextC AddComponent(TransformC _tc, string _text, float _fontSize, bool _isDynamic, bool _isMultiline, float _textAreaAlignX, float _textAreaAlignY, float _textAreaWidth, float _textAreaHeight, Align _textHorizontalAlign, Align _textVerticalAlign, float _marginLeft, float _marginRight, float _marginTop, float _marginBottom, float _offsetX, float _offsetY)
	{
		int num = m_components.AddItem();
		TextC textC = m_components.m_array[num];
		textC.entityIndex = _tc.entityIndex;
		textC.active = true;
		textC.text = _text;
		textC.fontSize = _fontSize;
		textC.textAreaAlignX = _textAreaAlignX;
		textC.textAreaAlignY = _textAreaAlignY;
		textC.textAreaWidth = _textAreaWidth;
		textC.textAreaHeight = _textAreaHeight;
		textC.textHorizontalAlign = _textHorizontalAlign;
		textC.textVerticalAlign = _textVerticalAlign;
		textC.isDynamic = _isDynamic;
		textC.isMultiline = _isMultiline;
		textC.marginLeft = _marginLeft;
		textC.marginRight = _marginRight;
		textC.marginTop = _marginTop;
		textC.marginBottom = _marginBottom;
		textC.update = false;
		textC.TC = _tc;
		textC.textAreaOffsetX = _offsetX;
		textC.textAreaOffsetY = _offsetY;
		textC.contentTC = TransformS.AddComponent(_tc.entityIndex);
		TransformS.ParentComponent(textC.contentTC, textC.TC);
		TransformS.SetPosition(textC.contentTC, Vector3.zero);
		textC.textWidth = 0f;
		textC.textHeight = 0f;
		if (!textC.isMultiline)
		{
			CreateSingleLineText(textC);
		}
		else
		{
			CreateText(textC);
		}
		EntityManager.m_entities.m_array[_tc.entityIndex].components.Add(textC);
		return textC;
	}

	public static TextC AddSingleLineComponent(TransformC _tc, string _text, float _fontSize, Align _hAlign, Align _vAlign)
	{
		return AddComponent(_tc, _text, _fontSize, true, false, 0.5f, 0.5f, 0f, 0f, _hAlign, _vAlign, 0f, 0f, 0f, 0f, 0f, 0f);
	}

	public static void RemoveComponent(TextC _t)
	{
		RemoveComponent(_t, false);
	}

	public static void RemoveComponent(TextC _t, bool _clear)
	{
		_t.active = false;
		_t.TC = null;
		if (_t.gameObject != null)
		{
			UnityEngine.Object.Destroy(_t.gameObject);
		}
		if (_clear)
		{
			List<IComponent> componentsByEntityIndex = EntityManager.GetComponentsByEntityIndex(ComponentType.Sprite, _t.entityIndex);
			for (int i = 0; i < componentsByEntityIndex.Count; i++)
			{
				SpriteS.RemoveComponent(componentsByEntityIndex[i] as SpriteC);
			}
		}
		m_components.RemoveItem(_t.index);
		EntityManager.m_entities.m_array[_t.entityIndex].components.Remove(_t);
		_t.entityIndex = -1;
	}

	public static void ClearTextComponent(TextC _t)
	{
		List<IComponent> componentsByEntityIndex = EntityManager.GetComponentsByEntityIndex(ComponentType.Sprite, _t.entityIndex);
		for (int i = 0; i < componentsByEntityIndex.Count; i++)
		{
			SpriteS.RemoveComponent(componentsByEntityIndex[i] as SpriteC);
		}
	}

	public static void ChangeText(TextC _t, string _text)
	{
		ClearTextComponent(_t);
		_t.text = _text;
		if (!_t.isMultiline)
		{
			CreateSingleLineText(_t);
		}
		else
		{
			CreateText(_t);
		}
	}

	public static void SetTextAreaOffset(TextC _t, float _offsetX, float _offsetY)
	{
		_t.textAreaOffsetX = _offsetX;
		_t.textAreaOffsetY = _offsetY;
		ClearTextComponent(_t);
		CreateText(_t);
	}

	private static void CreateSingleLineText(TextC _t)
	{
		if (m_style == null)
		{
			Debug.LogError("set style first");
		}
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		for (int i = 0; i < _t.text.Length; i++)
		{
			int num = _t.text[i];
			if (num < 256)
			{
				Font p_font = m_style.p_font;
				Vector3 zero3 = Vector3.zero;
				zero2.x += (float)p_font.characters[num].xAdvance * m_style.xScale * _t.fontSize;
			}
		}
		if (_t.textHorizontalAlign == Align.Right)
		{
			zero.x -= zero2.x;
		}
		else if (_t.textHorizontalAlign == Align.Center)
		{
			zero.x -= zero2.x * 0.5f;
		}
		if (_t.textVerticalAlign == Align.Middle || _t.textVerticalAlign == Align.Center)
		{
			TransformS.SetPosition(_t.contentTC, Vector3.up * m_style.p_font.lineHeight * m_style.yScale * _t.fontSize * 0.5f);
		}
		else if (_t.textVerticalAlign == Align.Top)
		{
			TransformS.SetPosition(_t.contentTC, Vector3.up * (m_style.p_font.lineHeight - m_style.p_font.baseline) * m_style.yScale * _t.fontSize);
		}
		else if (_t.textVerticalAlign == Align.Bottom)
		{
			TransformS.SetPosition(_t.contentTC, Vector3.up * m_style.p_font.lineHeight * m_style.yScale * _t.fontSize);
		}
		for (int j = 0; j < _t.text.Length; j++)
		{
			int num2 = _t.text[j];
			if (num2 < 256)
			{
				Font p_font2 = m_style.p_font;
				Vector3 zero4 = Vector3.zero;
				zero4.x = (float)p_font2.characters[num2].xOffset * _t.fontSize + (float)p_font2.characters[num2].width * 0.5f;
				zero4.y = (float)(-p_font2.characters[num2].yOffset) * _t.fontSize - (float)p_font2.characters[num2].height * _t.fontSize * 0.5f;
				zero4.x *= m_style.xScale * _t.fontSize;
				Frame frame = new Frame(p_font2.characters[num2].x, p_font2.characters[num2].y, p_font2.characters[num2].width, p_font2.characters[num2].height);
				SpriteC spriteC = SpriteS.AddComponent(_t.contentTC, frame, p_font2.p_spriteSheet);
				SpriteS.SetDimensions(spriteC, frame.width * m_style.xScale * _t.fontSize, frame.height * m_style.yScale * _t.fontSize);
				SpriteS.SetColor(spriteC, m_style.color);
				SpriteS.SetOffset(spriteC, zero + zero4, 0f);
				zero.x += (float)p_font2.characters[num2].xAdvance * m_style.xScale * _t.fontSize;
			}
		}
		zero.y -= m_style.lineHeight * m_style.yScale * _t.fontSize;
		_t.textWidth = zero2.x;
		_t.textHeight = 0f - zero.y;
	}

	private static void CreateText(TextC _t)
	{
		if (m_style == null)
		{
			Debug.LogError("set style first");
		}
		string[] array = _t.text.Split('\n');
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		Vector3 vector = new Vector3(_t.textAreaOffsetX, _t.textAreaOffsetY, 0f);
		float x = (0f - _t.textAreaWidth) * _t.textAreaAlignX + _t.marginLeft;
		float num = _t.textAreaHeight * (1f - _t.textAreaAlignY) - _t.marginTop;
		if (_t.textVerticalAlign == Align.Middle)
		{
			float num2 = _t.textAreaHeight - _t.marginTop - _t.marginBottom;
			float num3 = (float)array.Length * m_style.lineHeight * m_style.yScale * _t.fontSize;
			num -= (num2 - num3) * 0.5f;
		}
		else if (_t.textVerticalAlign == Align.Bottom)
		{
			float num4 = _t.textAreaHeight - _t.marginTop - _t.marginBottom;
			float num5 = (float)array.Length * m_style.lineHeight * m_style.yScale * _t.fontSize;
			num -= num4 - num5;
		}
		zero.y = num;
		string[] array2 = array;
		foreach (string text in array2)
		{
			int num6 = 0;
			bool flag = false;
			while (!flag)
			{
				flag = true;
				zero.x = x;
				zero2.x = _t.marginLeft;
				int num7 = 0;
				int num8 = 0;
				int num9 = 0;
				string text2 = string.Empty;
				float x2 = 0f;
				float num10 = 0f;
				float num11 = 0f;
				float num12 = 0f;
				for (int j = num6; j < text.Length; j++)
				{
					int num13 = text[j];
					if (num13 >= 256)
					{
						continue;
					}
					Font p_font = m_style.p_font;
					Vector3 zero3 = Vector3.zero;
					if (num13 == 32)
					{
						num8 = num7;
						x2 = zero2.x;
						num9++;
					}
					num11 = num10;
					num10 = zero2.x;
					zero2.x += (float)p_font.characters[num13].xAdvance * m_style.xScale * _t.fontSize;
					if (zero2.x > _t.textAreaWidth - _t.marginRight)
					{
						if (num9 > 0)
						{
							zero2.x = x2;
							text2 = text.Substring(num6, num8);
							num6 += num8 + 1;
							num9--;
						}
						else
						{
							zero2.x = num11;
							text2 = text.Substring(num6, num7 - 1);
							num6 += num7 - 1;
						}
						flag = false;
						break;
					}
					num7++;
				}
				if (flag)
				{
					text2 = text.Substring(num6, num7);
				}
				if (_t.textHorizontalAlign == Align.Right)
				{
					zero.x += _t.textAreaWidth - _t.marginRight - zero2.x;
				}
				else if (_t.textHorizontalAlign == Align.Center)
				{
					zero.x += (_t.textAreaWidth - _t.marginRight - zero2.x) * 0.5f;
				}
				else if (_t.textHorizontalAlign == Align.Justified && !flag)
				{
					num12 = ((num9 != 0) ? ((_t.textAreaWidth - _t.marginRight - zero2.x) / (float)num9) : ((_t.textAreaWidth - _t.marginRight - zero2.x) / (float)(num7 - 1)));
				}
				string text3 = text2;
				foreach (int num14 in text3)
				{
					if (num14 < 256)
					{
						Font p_font2 = m_style.p_font;
						Vector3 zero4 = Vector3.zero;
						zero4.x = (float)p_font2.characters[num14].xOffset * _t.fontSize + (float)p_font2.characters[num14].width * 0.5f;
						zero4.y = (float)(-p_font2.characters[num14].yOffset) * _t.fontSize - (float)p_font2.characters[num14].height * _t.fontSize * 0.5f;
						zero4.x *= m_style.xScale * _t.fontSize;
						Frame frame = new Frame(p_font2.characters[num14].x, p_font2.characters[num14].y, p_font2.characters[num14].width, p_font2.characters[num14].height);
						SpriteC spriteC = SpriteS.AddComponent(_t.contentTC, frame, p_font2.p_spriteSheet);
						SpriteS.SetDimensions(spriteC, frame.width * m_style.xScale * _t.fontSize, frame.height * m_style.yScale * _t.fontSize);
						SpriteS.SetColor(spriteC, m_style.color);
						SpriteS.SetOffset(spriteC, zero + zero4 + vector, 0f);
						zero.x += (float)p_font2.characters[num14].xAdvance * m_style.xScale * _t.fontSize;
						if (num9 == 0)
						{
							zero.x += num12;
						}
						else if (num14 == 32)
						{
							zero.x += num12;
						}
					}
				}
				zero.y -= m_style.lineHeight * m_style.yScale * _t.fontSize;
				_t.textWidth = zero.x;
				_t.textHeight = 0f - zero.y;
			}
		}
	}

	public static void Update()
	{
	}
}
