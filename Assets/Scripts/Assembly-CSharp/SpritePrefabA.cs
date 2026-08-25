using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpritePrefabA
{
	public static Hashtable m_prefabs;

	public static Hashtable m_animations;

	public static void ParseAnimation(IResource _resource, string _key)
	{
		if (m_animations == null)
		{
			m_animations = new Hashtable();
		}
		Hashtable hashtable = new Hashtable();
		m_animations.Add(_key, hashtable);
		Hashtable hashtable2 = null;
		string text = (_resource.resourceObject as TextAsset).text;
		string[] separator = new string[1] { "\n" };
		string[] array = text.Split(separator, StringSplitOptions.None);
		SpritePrefabAnimation spritePrefabAnimation = new SpritePrefabAnimation();
		string empty = string.Empty;
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Replace("\r", string.Empty);
			string[] separator2 = new string[1] { "=" };
			string[] array2 = array[i].Split(separator2, StringSplitOptions.None);
			if (array[i] != string.Empty)
			{
				string[] separator3 = new string[1] { "," };
				string[] array3 = null;
				if (array2.Length > 1)
				{
					array3 = array2[1].Split(separator3, StringSplitOptions.None);
				}
				if (array2[0] == "animationName")
				{
					empty = array2[1];
					spritePrefabAnimation = new SpritePrefabAnimation();
					spritePrefabAnimation.name = empty;
					hashtable.Add(empty, spritePrefabAnimation);
				}
				else if (array2[0] == "frameCount")
				{
					spritePrefabAnimation.frames = int.Parse(array2[1]);
					hashtable2 = m_prefabs[_key] as Hashtable;
					spritePrefabAnimation.nodeCount = hashtable2.Count;
					spritePrefabAnimation.positions = new Vector3[spritePrefabAnimation.frames * spritePrefabAnimation.nodeCount];
					spritePrefabAnimation.rotations = new Vector3[spritePrefabAnimation.frames * spritePrefabAnimation.nodeCount];
					spritePrefabAnimation.scales = new Vector3[spritePrefabAnimation.frames * spritePrefabAnimation.nodeCount];
					spritePrefabAnimation.visibilities = new bool[spritePrefabAnimation.frames * spritePrefabAnimation.nodeCount];
				}
				else if (array2[0] == "step")
				{
					spritePrefabAnimation.step = int.Parse(array2[1]);
				}
				else if (array2[0] == "frame")
				{
					num = int.Parse(array2[1]);
				}
				else
				{
					string[] separator4 = new string[1] { " " };
					string[] array4 = array[i].Split(separator4, StringSplitOptions.None);
					SpritePrefabNode spritePrefabNode = hashtable2[array4[0]] as SpritePrefabNode;
					spritePrefabAnimation.positions[num * spritePrefabAnimation.nodeCount + spritePrefabNode.index] = new Vector3(float.Parse(array4[1]), float.Parse(array4[2]), float.Parse(array4[3]));
					spritePrefabAnimation.rotations[num * spritePrefabAnimation.nodeCount + spritePrefabNode.index] = new Vector3(float.Parse(array4[4]), float.Parse(array4[5]), float.Parse(array4[6]));
					spritePrefabAnimation.scales[num * spritePrefabAnimation.nodeCount + spritePrefabNode.index] = new Vector3(float.Parse(array4[7]), float.Parse(array4[8]), float.Parse(array4[9]));
					int num2 = int.Parse(array4[10]);
					spritePrefabAnimation.visibilities[num * spritePrefabAnimation.nodeCount + spritePrefabNode.index] = num2 == 1;
				}
			}
		}
	}

	public static void Parse(IResource _resource, string _key, TiledSpriteSheet _tiledSheet)
	{
		Hashtable hashtable = Parse(_resource, _key);
		foreach (DictionaryEntry item in hashtable)
		{
			SpritePrefabNode spritePrefabNode = item.Value as SpritePrefabNode;
			spritePrefabNode.tiledSpriteSheet = _tiledSheet;
		}
	}

	public static void Parse(IResource _resource, string _key, SpriteSheet _sheet)
	{
		Parse(_resource, _key, _sheet, false);
	}

	public static void Parse(IResource _resource, string _key, SpriteSheet _sheet, bool _inverseZSort)
	{
		Hashtable hashtable = Parse(_resource, _key);
		foreach (DictionaryEntry item in hashtable)
		{
			SpritePrefabNode spritePrefabNode = item.Value as SpritePrefabNode;
			spritePrefabNode.spriteSheet = _sheet;
			if (_inverseZSort)
			{
				spritePrefabNode.sortValue *= -1f;
			}
		}
	}

	public static void Parse(IResource _resource, SpriteSheet _sheet)
	{
		Parse(_resource, _resource.identifier, _sheet, false);
	}

	public static void Parse(IResource _resource, SpriteSheet _sheet, bool _inverseZSort)
	{
		Parse(_resource, _resource.identifier, _sheet, _inverseZSort);
	}

	private static Hashtable Parse(IResource _resource, string _key)
	{
		if (m_prefabs == null)
		{
			m_prefabs = new Hashtable();
		}
		Hashtable hashtable = new Hashtable();
		m_prefabs.Add(_key, hashtable);
		string text = (_resource.resourceObject as TextAsset).text;
		string[] separator = new string[1] { "\n" };
		string[] array = text.Split(separator, StringSplitOptions.None);
		SpritePrefabNode spritePrefabNode = new SpritePrefabNode();
		string empty = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Replace("\r", string.Empty);
			string[] separator2 = new string[1] { "=" };
			string[] array2 = array[i].Split(separator2, StringSplitOptions.None);
			if (!(array[i] != string.Empty))
			{
				continue;
			}
			string[] separator3 = new string[1] { "," };
			string[] array3 = array2[1].Split(separator3, StringSplitOptions.None);
			if (array2[0] == "name")
			{
				empty = array2[1];
				spritePrefabNode = new SpritePrefabNode();
				spritePrefabNode.name = empty;
				hashtable.Add(empty, spritePrefabNode);
			}
			else if (array2[0] == "index")
			{
				spritePrefabNode.index = int.Parse(array2[1]);
			}
			else if (array2[0] == "parentIndex")
			{
				spritePrefabNode.parentIndex = int.Parse(array2[1]);
			}
			else if (array2[0] == "defaultPosition")
			{
				spritePrefabNode.localPosition = new Vector3(float.Parse(array3[0]), float.Parse(array3[1]), 0f - float.Parse(array3[2]));
			}
			else if (array2[0] == "defaultRotation")
			{
				spritePrefabNode.localRotation = new Vector3(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]));
			}
			else if (array2[0] == "localScale")
			{
				spritePrefabNode.localScale = new Vector3(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]));
			}
			else if (array2[0] == "globalPosition")
			{
				spritePrefabNode.globalPosition = new Vector3(float.Parse(array3[0]), float.Parse(array3[1]), 0f - float.Parse(array3[2]));
			}
			else if (array2[0] == "globalRotation")
			{
				spritePrefabNode.globalRotation = new Vector3(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]));
			}
			else if (array2[0] == "prop")
			{
				spritePrefabNode.isProp = int.Parse(array2[1]);
			}
			else if (array2[0] == "notVisual")
			{
				spritePrefabNode.notVisual = int.Parse(array2[1]);
			}
			else if (array2[0] == "dictateRotation")
			{
				spritePrefabNode.dictateRotation = int.Parse(array2[1]);
			}
			else if (array2[0] == "dictatedRotation")
			{
				spritePrefabNode.dictatedRotation = float.Parse(array2[1]);
			}
			else if (array2[0] == "hasMotor")
			{
				spritePrefabNode.hasMotor = int.Parse(array2[1]);
			}
			else if (array2[0] == "motorStrength")
			{
				spritePrefabNode.motorStrength = float.Parse(array2[1]);
			}
			else if (array2[0] == "motorRate")
			{
				spritePrefabNode.motorRate = float.Parse(array2[1]);
			}
			else if (array2[0] == "hasSuspension")
			{
				spritePrefabNode.hasSuspension = int.Parse(array2[1]);
			}
			else if (array2[0] == "suspensionStrength")
			{
				spritePrefabNode.suspensionStrength = float.Parse(array2[1]);
			}
			else if (array2[0] == "suspensionDamp")
			{
				spritePrefabNode.suspensionDamp = float.Parse(array2[1]);
			}
			else if (array2[0] == "suspensionDepth")
			{
				spritePrefabNode.suspensionDepth = float.Parse(array2[1]);
			}
			else if (array2[0] == "eye")
			{
				spritePrefabNode.isEye = int.Parse(array2[1]);
			}
			else if (array2[0] == "leg")
			{
				spritePrefabNode.isLeg = int.Parse(array2[1]);
			}
			else if (array2[0] == "arm")
			{
				spritePrefabNode.isArm = int.Parse(array2[1]);
			}
			else if (array2[0] == "head")
			{
				spritePrefabNode.isHead = int.Parse(array2[1]);
			}
			else if (array2[0] == "tire")
			{
				spritePrefabNode.isTire = int.Parse(array2[1]);
			}
			else if (array2[0] == "crawler")
			{
				spritePrefabNode.isCrawler = int.Parse(array2[1]);
			}
			else if (array2[0] == "physics")
			{
				spritePrefabNode.hasPhysics = int.Parse(array2[1]);
			}
			else if (array2[0] == "static")
			{
				spritePrefabNode.isStatic = int.Parse(array2[1]);
			}
			else if (array2[0] == "sensor")
			{
				spritePrefabNode.isSensor = int.Parse(array2[1]);
			}
			else if (array2[0] == "transformDictates")
			{
				spritePrefabNode.transformDictates = int.Parse(array2[1]);
			}
			else if (array2[0] == "colliderShape")
			{
				spritePrefabNode.colliderShape = int.Parse(array2[1]);
			}
			else if (array2[0] == "elasticity")
			{
				spritePrefabNode.elasticity = float.Parse(array2[1]);
			}
			else if (array2[0] == "friction")
			{
				spritePrefabNode.friction = float.Parse(array2[1]);
			}
			else if (array2[0] == "linearDamp")
			{
				spritePrefabNode.linearDamp = new Vector3(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]));
			}
			else if (array2[0] == "angularDamp")
			{
				spritePrefabNode.angularDamp = float.Parse(array2[1]);
			}
			else if (array2[0] == "gravity")
			{
				spritePrefabNode.gravity = new Vector3(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]));
			}
			else if (array2[0] == "reactToWind")
			{
				spritePrefabNode.reactToWind = int.Parse(array2[1]);
			}
			else if (array2[0] == "mass")
			{
				spritePrefabNode.mass = float.Parse(array2[1]);
			}
			else if (array2[0] == "rotarySpring")
			{
				spritePrefabNode.hasRotarySpring = int.Parse(array2[1]);
			}
			else if (array2[0] == "rotarySpringStrength")
			{
				spritePrefabNode.rotarySpringStrength = float.Parse(array2[1]);
			}
			else if (array2[0] == "rotarySpringDamp")
			{
				spritePrefabNode.rotarySpringDamp = float.Parse(array2[1]);
			}
			else if (array2[0] == "rotaryLimit")
			{
				spritePrefabNode.hasRotaryLimits = int.Parse(array2[1]);
			}
			else if (array2[0] == "minRotaryLimit")
			{
				spritePrefabNode.minRotaryLimit = float.Parse(array2[1]);
			}
			else if (array2[0] == "maxRotaryLimit")
			{
				spritePrefabNode.maxRotaryLimit = float.Parse(array2[1]);
			}
			else if (array2[0] == "maxAngularVelocity")
			{
				spritePrefabNode.maxAngularVelocity = float.Parse(array2[1]);
			}
			else if (array2[0] == "visibility")
			{
				spritePrefabNode.visibility = int.Parse(array2[1]);
			}
			else if (array2[0] == "vertexCount")
			{
				spritePrefabNode.vertices = new Vector3[int.Parse(array2[1])];
				if (spritePrefabNode.vertices.Length == 0)
				{
					spritePrefabNode.uvs = new Vector2[0];
				}
			}
			else if (array2[0] == "vertices")
			{
				for (int j = 0; j < spritePrefabNode.vertices.Length; j++)
				{
					spritePrefabNode.vertices[j] = new Vector3(float.Parse(array3[j * 3]), float.Parse(array3[j * 3 + 1]), float.Parse(array3[j * 3 + 2]));
				}
			}
			else if (array2[0] == "collisionVertexCount")
			{
				spritePrefabNode.collisionVertices = new Vector3[int.Parse(array2[1])];
			}
			else if (array2[0] == "collisionVertices")
			{
				for (int k = 0; k < spritePrefabNode.collisionVertices.Length; k++)
				{
					spritePrefabNode.collisionVertices[k] = new Vector3(float.Parse(array3[k * 3]), float.Parse(array3[k * 3 + 1]), float.Parse(array3[k * 3 + 2]));
				}
			}
			else if (array2[0] == "UVCount")
			{
				spritePrefabNode.uvs = new Vector2[int.Parse(array2[1])];
			}
			else if (array2[0] == "UVs")
			{
				for (int l = 0; l < spritePrefabNode.uvs.Length; l++)
				{
					spritePrefabNode.uvs[l] = new Vector2(float.Parse(array3[l * 2]), float.Parse(array3[l * 2 + 1]));
				}
			}
		}
		foreach (DictionaryEntry item in hashtable)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 0f;
			float num10 = 0f;
			spritePrefabNode = item.Value as SpritePrefabNode;
			Vector3 localPosition = spritePrefabNode.localPosition;
			for (int m = 0; m < spritePrefabNode.vertices.Length; m++)
			{
				Vector3 vector = spritePrefabNode.vertices[m];
				if (m == 0)
				{
					num = vector.x;
					num2 = vector.x;
					num4 = vector.y;
					num3 = vector.y;
					num6 = vector.z;
					num5 = vector.z;
				}
				else
				{
					num = Mathf.Min(vector.x, num);
					num2 = Mathf.Max(vector.x, num2);
					num4 = Mathf.Min(vector.y, num4);
					num3 = Mathf.Max(vector.y, num3);
					num6 = Mathf.Min(vector.z, num6);
					num5 = Mathf.Max(vector.z, num5);
				}
			}
			for (int n = 0; n < spritePrefabNode.uvs.Length; n++)
			{
				Vector2 vector2 = spritePrefabNode.uvs[n];
				if (n == 0)
				{
					num7 = vector2.x;
					num8 = vector2.x;
					num10 = vector2.y;
					num9 = vector2.y;
				}
				else
				{
					num7 = Mathf.Min(vector2.x, num7);
					num8 = Mathf.Max(vector2.x, num8);
					num10 = Mathf.Min(vector2.y, num10);
					num9 = Mathf.Max(vector2.y, num9);
				}
			}
			spritePrefabNode.width = num2 - num;
			spritePrefabNode.height = num3 - num4;
			if (spritePrefabNode.width == 0f)
			{
				num = -5f;
				num2 = 5f;
				spritePrefabNode.width = 10f;
			}
			if (spritePrefabNode.height == 0f)
			{
				num4 = -5f;
				num3 = 5f;
				spritePrefabNode.height = 10f;
			}
			spritePrefabNode.localCenter = new Vector3(num + spritePrefabNode.width * 0.5f, num4 + spritePrefabNode.height * 0.5f, num6 + (num5 - num6) * 0.5f);
			spritePrefabNode.globalCenter = spritePrefabNode.globalPosition + spritePrefabNode.localCenter;
			spritePrefabNode.uvX = num7;
			spritePrefabNode.uvY = 1f - num9;
			spritePrefabNode.uvWidth = num8 - num7;
			spritePrefabNode.uvHeight = num9 - num10;
			spritePrefabNode.sortValue = 0f - spritePrefabNode.globalPosition.z;
			spritePrefabNode.rotarySpring = IntPtr.Zero;
			spritePrefabNode.pivotJoint = IntPtr.Zero;
		}
		return hashtable;
	}

	private static SpritePrefabNode CopyNode(SpritePrefabNode _node)
	{
		SpritePrefabNode spritePrefabNode = new SpritePrefabNode();
		spritePrefabNode.angularDamp = _node.angularDamp;
		spritePrefabNode.colliderShape = _node.colliderShape;
		spritePrefabNode.globalCenter = _node.globalCenter;
		spritePrefabNode.globalPosition = _node.globalPosition;
		spritePrefabNode.globalRotation = _node.globalRotation;
		spritePrefabNode.gravity = _node.gravity;
		spritePrefabNode.hasPhysics = _node.hasPhysics;
		spritePrefabNode.hasRotaryLimits = _node.hasRotaryLimits;
		spritePrefabNode.hasRotarySpring = _node.hasRotarySpring;
		spritePrefabNode.height = _node.height;
		spritePrefabNode.index = _node.index;
		spritePrefabNode.isProp = _node.isProp;
		spritePrefabNode.notVisual = _node.notVisual;
		spritePrefabNode.isEye = _node.isEye;
		spritePrefabNode.isHead = _node.isHead;
		spritePrefabNode.isArm = _node.isArm;
		spritePrefabNode.isLeg = _node.isLeg;
		spritePrefabNode.isTire = _node.isTire;
		spritePrefabNode.isCrawler = _node.isCrawler;
		spritePrefabNode.isSensor = _node.isSensor;
		spritePrefabNode.hasMotor = _node.hasMotor;
		spritePrefabNode.motorStrength = _node.motorStrength;
		spritePrefabNode.motorRate = _node.motorRate;
		spritePrefabNode.hasSuspension = _node.hasSuspension;
		spritePrefabNode.suspensionStrength = _node.suspensionStrength;
		spritePrefabNode.suspensionDamp = _node.suspensionDamp;
		spritePrefabNode.suspensionDepth = _node.suspensionDepth;
		spritePrefabNode.transformDictates = _node.transformDictates;
		spritePrefabNode.dictateRotation = _node.dictateRotation;
		spritePrefabNode.dictatedRotation = _node.dictatedRotation;
		spritePrefabNode.isStatic = _node.isStatic;
		spritePrefabNode.linearDamp = _node.linearDamp;
		spritePrefabNode.localCenter = _node.localCenter;
		spritePrefabNode.localPosition = _node.localPosition;
		spritePrefabNode.localRotation = _node.localRotation;
		spritePrefabNode.localScale = _node.localScale;
		spritePrefabNode.mass = _node.mass;
		spritePrefabNode.elasticity = _node.elasticity;
		spritePrefabNode.friction = _node.friction;
		spritePrefabNode.maxRotaryLimit = _node.maxRotaryLimit;
		spritePrefabNode.minRotaryLimit = _node.minRotaryLimit;
		spritePrefabNode.maxAngularVelocity = _node.maxAngularVelocity;
		spritePrefabNode.name = _node.name;
		spritePrefabNode.parentIndex = _node.parentIndex;
		spritePrefabNode.rotarySpringDamp = _node.rotarySpringDamp;
		spritePrefabNode.rotarySpringStrength = _node.rotarySpringStrength;
		spritePrefabNode.sortValue = _node.sortValue;
		spritePrefabNode.spriteSheet = _node.spriteSheet;
		spritePrefabNode.tiledSpriteSheet = _node.tiledSpriteSheet;
		spritePrefabNode.uvHeight = _node.uvHeight;
		spritePrefabNode.uvs = _node.uvs;
		spritePrefabNode.uvWidth = _node.uvWidth;
		spritePrefabNode.uvX = _node.uvX;
		spritePrefabNode.uvY = _node.uvY;
		spritePrefabNode.vertices = _node.vertices;
		spritePrefabNode.width = _node.width;
		spritePrefabNode.visibility = _node.visibility;
		spritePrefabNode.rotarySpring = IntPtr.Zero;
		spritePrefabNode.rotaryLimitJoint = IntPtr.Zero;
		spritePrefabNode.pivotJoint = IntPtr.Zero;
		return spritePrefabNode;
	}

	public static GESpritePrefabC Assemble(Vector3 _pos, string _key, string[] _tags, ColliderType _colliderType, uint _colliderGroup, uint _colliderLayer, float _depthOffset)
	{
		return Assemble(-1, _pos, _key, _tags, _colliderType, _colliderGroup, _colliderLayer, _depthOffset);
	}

	public static GESpritePrefabC Assemble(int _entityIndex, Vector3 _pos, string _key, string[] _tags, ColliderType _colliderType, uint _colliderGroup, uint _colliderLayer, float _sortOffset)
	{
		return Assemble(_entityIndex, _pos, _key, _tags, _colliderType, _colliderGroup, _colliderLayer, _sortOffset, null);
	}

	public static GESpritePrefabC Assemble(int _entityIndex, Vector3 _pos, string _key, string[] _tags, ColliderType _colliderType, uint _colliderGroup, uint _colliderLayer, float _sortOffset, SpriteSheet _sheet)
	{
		List<SpritePrefabNode> list = new List<SpritePrefabNode>();
		List<GESpritePrefabC> list2 = new List<GESpritePrefabC>();
		if (m_prefabs != null)
		{
			Hashtable hashtable = m_prefabs[_key] as Hashtable;
			Hashtable hashtable2 = new Hashtable();
			SpritePrefabNode[] array = new SpritePrefabNode[hashtable.Count];
			Entity entity = ((_entityIndex != -1) ? EntityManager.m_entities.m_array[_entityIndex] : EntityManager.AddEntity(_tags));
			foreach (DictionaryEntry item2 in hashtable)
			{
				SpritePrefabNode spritePrefabNode = CopyNode(item2.Value as SpritePrefabNode);
				array[spritePrefabNode.index] = spritePrefabNode;
				hashtable2.Add(spritePrefabNode.name, spritePrefabNode);
				spritePrefabNode.TC = TransformS.AddComponent(entity);
				spritePrefabNode.TC.transform.position = _pos;
				if (spritePrefabNode.hasPhysics == 1)
				{
					bool flag = false;
					bool sensor = false;
					if (spritePrefabNode.isStatic == 1)
					{
						flag = true;
					}
					if (spritePrefabNode.isSensor == 1)
					{
						sensor = true;
					}
					if (spritePrefabNode.isProp == 1)
					{
						spritePrefabNode.CMC = ChipmunkS.AddInactiveComponent(spritePrefabNode.TC, spritePrefabNode.transformDictates == 1, _colliderType, _colliderGroup, 0u, flag && spritePrefabNode.transformDictates == 0, flag && spritePrefabNode.transformDictates == 1);
					}
					else if (spritePrefabNode.isTire == 1 || spritePrefabNode.isCrawler == 1)
					{
						spritePrefabNode.CMC = ChipmunkS.AddInactiveComponent(spritePrefabNode.TC, spritePrefabNode.transformDictates == 1, (ColliderType)10, _colliderGroup, _colliderLayer, false, false);
					}
					else
					{
						spritePrefabNode.CMC = ChipmunkS.AddInactiveComponent(spritePrefabNode.TC, spritePrefabNode.transformDictates == 1, _colliderType, _colliderGroup, _colliderLayer, flag && spritePrefabNode.transformDictates == 0, flag && spritePrefabNode.transformDictates == 1);
					}
					IntPtr bodyPtr = ChipmunkWrapper.AddBodyWithCustomProperties(spritePrefabNode.CMC.isStatic, spritePrefabNode.CMC.isRogue, _pos + spritePrefabNode.globalPosition + spritePrefabNode.localCenter, spritePrefabNode.CMC.index, spritePrefabNode.CMC.colliderType, spritePrefabNode.linearDamp, spritePrefabNode.angularDamp, spritePrefabNode.gravity);
					if (spritePrefabNode.colliderShape == 1)
					{
						ChipmunkWrapper.AddCircleShape(bodyPtr, Vector2.zero, spritePrefabNode.mass, spritePrefabNode.width * 0.5f, spritePrefabNode.elasticity, spritePrefabNode.friction, spritePrefabNode.CMC.colliderGroup, spritePrefabNode.CMC.colliderLayer, sensor);
					}
					else if (spritePrefabNode.colliderShape == 0)
					{
						ChipmunkWrapper.AddBoxShape(bodyPtr, Vector2.zero, spritePrefabNode.mass, spritePrefabNode.width, spritePrefabNode.height, spritePrefabNode.elasticity, spritePrefabNode.friction, spritePrefabNode.CMC.colliderGroup, spritePrefabNode.CMC.colliderLayer, sensor);
					}
					ChipmunkS.ActivateChipmunkComponent(spritePrefabNode.CMC, bodyPtr);
				}
			}
			for (int i = 0; i < array.Length; i++)
			{
				SpritePrefabNode spritePrefabNode2 = array[i];
				if (spritePrefabNode2.parentIndex != -1)
				{
					SpritePrefabNode spritePrefabNode3 = array[array[i].parentIndex];
					if (spritePrefabNode2.hasPhysics == 1)
					{
						if (spritePrefabNode2.hasSuspension == 1)
						{
							float x = Mathf.Sin((0f - spritePrefabNode2.globalRotation.z) * ((float)Math.PI / 180f));
							float y = Mathf.Cos((0f - spritePrefabNode2.globalRotation.z) * ((float)Math.PI / 180f));
							Vector3 vector = new Vector3(x, y, 0f);
							spritePrefabNode2.grooveJoint = ChipmunkWrapper.AddGrooveJoint(spritePrefabNode3.CMC.cpBodyPtr, spritePrefabNode2.CMC.cpBodyPtr, -spritePrefabNode3.localCenter + spritePrefabNode2.localPosition + vector * (0f - spritePrefabNode2.suspensionDepth), -spritePrefabNode3.localCenter + spritePrefabNode2.localPosition + vector * spritePrefabNode2.suspensionDepth, Vector2.zero);
							spritePrefabNode2.dampedSpring = ChipmunkWrapper.AddDampedSpring(spritePrefabNode3.CMC.cpBodyPtr, spritePrefabNode2.CMC.cpBodyPtr, -spritePrefabNode3.localCenter + spritePrefabNode2.localPosition, new Vector2(0f, 0f), 0f, spritePrefabNode2.suspensionStrength, spritePrefabNode2.suspensionDamp);
						}
						else if (spritePrefabNode3.hasPhysics == 1)
						{
							spritePrefabNode2.pivotJoint = ChipmunkWrapper.AddPivotJoint(spritePrefabNode3.CMC.cpBodyPtr, spritePrefabNode2.CMC.cpBodyPtr, _pos + spritePrefabNode2.globalPosition);
						}
						else
						{
							spritePrefabNode2.pivotJoint = IntPtr.Zero;
							spritePrefabNode2.CMC.transformComponentDictates = true;
							spritePrefabNode2.CMC.dictatePosition = true;
							spritePrefabNode2.CMC.dictateAngle = false;
						}
						if (spritePrefabNode3.hasPhysics == 1)
						{
							if (spritePrefabNode2.hasRotarySpring == 1)
							{
								spritePrefabNode2.rotarySpring = ChipmunkWrapper.AddDampedRotarySpring(spritePrefabNode3.CMC.cpBodyPtr, spritePrefabNode2.CMC.cpBodyPtr, (0f - spritePrefabNode2.globalRotation.z) * ((float)Math.PI / 180f), spritePrefabNode2.rotarySpringStrength, spritePrefabNode2.rotarySpringDamp);
							}
							if (spritePrefabNode2.hasRotaryLimits == 1)
							{
								spritePrefabNode2.rotaryLimitJoint = ChipmunkWrapper.AddRotaryLimitJoint(spritePrefabNode3.CMC.cpBodyPtr, spritePrefabNode2.CMC.cpBodyPtr, spritePrefabNode2.minRotaryLimit * ((float)Math.PI / 180f), spritePrefabNode2.maxRotaryLimit * ((float)Math.PI / 180f));
							}
						}
						if (spritePrefabNode2.isTire == 1)
						{
							if (spritePrefabNode2.hasMotor == 1)
							{
								spritePrefabNode2.motor = ChipmunkWrapper.AddSimpleMotor(ChipmunkWrapper.GetSpaceStaticBody(), spritePrefabNode2.CMC.cpBodyPtr, 0f, spritePrefabNode2.motorStrength);
							}
							else if (spritePrefabNode2.hasMotor == 1)
							{
								spritePrefabNode2.motor = ChipmunkWrapper.AddSimpleMotor(ChipmunkWrapper.GetSpaceStaticBody(), spritePrefabNode2.CMC.cpBodyPtr, spritePrefabNode2.motorRate, spritePrefabNode2.motorStrength);
							}
						}
						if (spritePrefabNode2.maxAngularVelocity > 0f)
						{
							ChipmunkWrapper.SetBodyVelocityLimits(spritePrefabNode2.CMC.cpBodyPtr, 999f, spritePrefabNode2.maxAngularVelocity * (float)Math.PI * 2f);
						}
					}
					else
					{
						TransformS.ParentComponent(spritePrefabNode2.TC, spritePrefabNode3.TC);
						TransformS.SetTransform(spritePrefabNode2.TC, spritePrefabNode2.localPosition - spritePrefabNode3.localCenter, spritePrefabNode2.localRotation);
						spritePrefabNode2.TC.forceRotation = spritePrefabNode2.dictateRotation == 1;
						spritePrefabNode2.TC.forcedRotation = Quaternion.Euler(new Vector3(0f, 0f, spritePrefabNode2.dictatedRotation));
						if (spritePrefabNode2.CMC != null)
						{
							spritePrefabNode2.CMC.transformComponentDictates = true;
						}
					}
				}
				else if (spritePrefabNode2.hasPhysics == 0)
				{
					TransformS.SetPosition(spritePrefabNode2.TC, _pos);
				}
				if (spritePrefabNode2.parentIndex == -1)
				{
					list.Add(spritePrefabNode2);
					if (spritePrefabNode2.CMC != null)
					{
						TransformS.SetTransform(spritePrefabNode2.TC, _pos + spritePrefabNode2.globalPosition + spritePrefabNode2.localCenter, spritePrefabNode2.localRotation, spritePrefabNode2.CMC.cpBodyPtr);
					}
					else
					{
						TransformS.SetTransform(spritePrefabNode2.TC, _pos + spritePrefabNode2.globalPosition + spritePrefabNode2.localCenter, spritePrefabNode2.localRotation);
					}
				}
				if (spritePrefabNode2.notVisual == 0)
				{
					int num = 0;
					int num2 = 0;
					SpriteSheet spriteSheet = _sheet;
					if (spriteSheet == null)
					{
						spriteSheet = spritePrefabNode2.spriteSheet;
					}
					if (spriteSheet != null)
					{
						num = spriteSheet.m_textureWidth;
						num2 = spriteSheet.m_textureHeight;
						spritePrefabNode2.SC = SpriteS.AddComponent(spritePrefabNode2.TC, new Frame(spritePrefabNode2.uvX * (float)num, spritePrefabNode2.uvY * (float)num2, spritePrefabNode2.uvWidth * (float)num, spritePrefabNode2.uvHeight * (float)num2, spritePrefabNode2.localScale.x == -1f, spritePrefabNode2.localScale.y == -1f), spriteSheet);
					}
					else
					{
						num = spritePrefabNode2.tiledSpriteSheet.m_textureWidth;
						num2 = spritePrefabNode2.tiledSpriteSheet.m_textureHeight;
						spritePrefabNode2.SC = SpriteS.AddComponent(spritePrefabNode2.TC, new Frame(spritePrefabNode2.uvX * (float)num, spritePrefabNode2.uvY * (float)num2, spritePrefabNode2.uvWidth * (float)num, spritePrefabNode2.uvHeight * (float)num2, spritePrefabNode2.localScale.x == -1f, spritePrefabNode2.localScale.y == -1f), spritePrefabNode2.tiledSpriteSheet);
					}
					SpriteS.SetDimensions(spritePrefabNode2.SC, spritePrefabNode2.width, spritePrefabNode2.height);
					SpriteS.SetSortValue(spritePrefabNode2.SC, spritePrefabNode2.sortValue + _sortOffset);
					if (spritePrefabNode2.visibility == 0)
					{
						SpriteS.SetVisibility(spritePrefabNode2.SC, false);
					}
					if (spritePrefabNode2.hasPhysics == 0 && spritePrefabNode2.parentIndex != -1)
					{
						SpriteS.SetOffset(spritePrefabNode2.SC, spritePrefabNode2.localCenter, 0f);
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				GESpritePrefabC item = GESpritePrefabS.AddComponent(list[j], array, hashtable2);
				list2.Add(item);
			}
		}
		return list2[0];
	}
}
