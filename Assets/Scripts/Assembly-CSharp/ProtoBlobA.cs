using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProtoBlobA
{
	[CompilerGenerated]
	private static Dictionary<string, int> _003C_003Ef__switch_0024map4;

	public static BlobC Assemble(EIC _eic, BlobData _data)
	{
		return Assemble(_eic, _data, null);
	}

	public static BlobC Assemble(EIC _eic, BlobData _data, Vector3[] _feetPos)
	{
		return Assemble(_data.position.ToVector3(), _feetPos, _data.radius, _data.friction, _data.elasticy, _data.minElasticy, _data.shapeDamp, _data.segmentLength);
	}

	public static BlobC Assemble(Vector3 _pos, Vector3[] _feetPos, float _radius, float _friction, float _elasticy, float _minElasticy, float _shapeDamp, float _segmentLength)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetPosition(transformC, _pos);
		BlobC blobC = BlobS.AddBlobComponent(entity, _pos);
		blobC.friction = _friction;
		blobC.elasticy = _elasticy;
		blobC.minElasticy = _minElasticy;
		blobC.shapeDamp = _shapeDamp;
		blobC.segmentLength = _segmentLength;
		blobC.radius = _radius;
		blobC.TAC = TouchAreaS.AddComponent(transformC, blobC.blobType.ToString(), blobC.radius * 1.25f, false, Main.camera, blobC);
		blobC.TAC.scaleByCameraDistance = true;
		TouchAreaS.AddTouchEventListener(blobC.TAC, HandleBlobTouches);
		float num = (float)Math.PI * 2f * _radius;
		int num2 = Mathf.RoundToInt(num / blobC.segmentLength);
		if (_feetPos != null)
		{
			num2 = _feetPos.Length;
		}
		List<IntPtr> list = new List<IntPtr>();
		List<ChipmunkC> list2 = new List<ChipmunkC>();
		List<Vector2> list3 = new List<Vector2>();
		float num3 = 360f / (float)num2;
		float num4 = (float)Math.PI * blobC.radius;
		num4 /= (float)num2;
		float num5 = 0f;
		for (int i = 0; i < num2; i++)
		{
			Vector2 vector = new Vector2(Mathf.Cos(num5 * ((float)Math.PI / 180f)), Mathf.Sin(num5 * ((float)Math.PI / 180f))) * blobC.radius;
			list3.Add(vector);
			if (_feetPos != null)
			{
				vector = _feetPos[i];
			}
			TransformC transformComponent = TransformS.AddComponent(entity);
			ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformComponent, false, (ColliderType)20);
			IntPtr bodyPtr = ChipmunkWrapper.AddCircleBody(collisionGroup: chipmunkC.colliderGroup = (uint)(BlobS.m_blobComponents.m_freeIndices[BlobS.m_blobComponents.m_freeCount] + 100), isStatic: false, isRogue: false, position: (Vector2)transformC.transform.position + vector, componentIndex: chipmunkC.index, offset: Vector2.zero, mass: 1f, radius: blobC.segmentLength * 0.5f, restitution: 0.5f, friction: blobC.friction, layers: 17895697u, sensor: false, _colliderType: (ColliderType)20);
			ChipmunkS.ActivateChipmunkComponent(chipmunkC, bodyPtr);
			ChipmunkWrapper.SetBodyVelocityLimits(chipmunkC.cpBodyPtr, 900f, 0f);
			chipmunkC.customComponent = blobC;
			list2.Add(chipmunkC);
			num5 = ToolBox.getCappedAngle(num5 + num3);
		}
		for (int j = 0; j < list2.Count; j++)
		{
			for (int k = j + 1; k < list2.Count + 1; k++)
			{
				IntPtr zero = IntPtr.Zero;
				if (j == list2.Count - 1)
				{
					Vector2 vector2 = list3[j] - list3[0];
					float num6 = Mathf.Max(blobC.radius * 2f - vector2.magnitude, blobC.minElasticy);
					zero = ChipmunkWrapper.AddDampedSpring(list2[j].cpBodyPtr, list2[0].cpBodyPtr, Vector2.zero, Vector2.zero, vector2.magnitude, num6 * num6, 40f);
				}
				else if (k < list2.Count)
				{
					Vector2 vector3 = list3[j] - list3[k];
					float num7 = Mathf.Max(blobC.radius * 2f - vector3.magnitude, blobC.minElasticy);
					if (k == j + 1)
					{
						zero = ChipmunkWrapper.AddDampedSpring(list2[j].cpBodyPtr, list2[k].cpBodyPtr, Vector2.zero, Vector2.zero, vector3.magnitude, num7 * num7, 40f);
					}
					else
					{
						zero = ChipmunkWrapper.AddDampedSpring(list2[j].cpBodyPtr, list2[k].cpBodyPtr, Vector2.zero, Vector2.zero, vector3.magnitude, num7 * blobC.elasticy, blobC.shapeDamp);
					}
				}
			}
		}
		blobC.feet = list2;
		if (GEState.editorMode)
		{
			for (int l = 0; l < blobC.feet.Count; l++)
			{
				ChipmunkWrapper.SetBodySensor(blobC.feet[l].cpBodyPtr, true);
				blobC.feet[l].transformComponentDictates = true;
			}
		}
		return blobC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		BlobData blobData = new BlobData();
		blobData.position = new Vertex3(_pos);
		blobData.rotation = new Vertex3(_rot);
		blobData.scale = new Vertex3(_sca);
		blobData.radius = 40f;
		blobData.friction = 0.25f;
		blobData.elasticy = 0.5f;
		blobData.minElasticy = 80f;
		blobData.shapeDamp = 1f;
		blobData.segmentLength = 8f;
		uint uniqueId = GES.GetUniqueId();
		blobData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, blobData, Main.camera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = false;
		eIC.isRotateable = false;
		eIC.isScaleable = false;
		eIC.isScaleUnified = false;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		BlobData data = _eic.data as BlobData;
		BlobC item = Assemble(_eic, data);
		_eic.gameComponents.Add(item);
	}

	public static void PopulatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		string[] array = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		BlobData blobData = _eic.data as BlobData;
	}

	public static void HandleShipPropertyChange(EventC _c)
	{
		BlobData blobData = EditorState.m_selection[0].data as BlobData;
		string identifier = _c.identifier;
		if (identifier != null)
		{
			if (_003C_003Ef__switch_0024map4 == null)
			{
				_003C_003Ef__switch_0024map4 = new Dictionary<string, int>(0);
			}
			int value;
			if (!_003C_003Ef__switch_0024map4.TryGetValue(identifier, out value))
			{
			}
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}

	public static void HandleBlobTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		BlobC blobC = _c.customComponent as BlobC;
		if (!GEState.editorMode && _c.touchEvent[_i] == TouchEvent.RollIn && blobC.launchTime + 0.5f < Main.m_gameTime && blobC.radius > 20f)
		{
			Vector2 deltaPosition = InputManager.m_touches[_c.touchIndex[_i]].deltaPosition;
			float cappedAngle = ToolBox.getCappedAngle(Mathf.Atan2(deltaPosition.y, deltaPosition.x) * 57.29578f + 180f);
			BlobLogic.SplitBlob(blobC, cappedAngle);
		}
	}
}
