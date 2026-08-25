using UnityEngine;

public interface IControlledComponent
{
	uint id { get; set; }

	TriggerType triggerType { get; set; }

	TransformC debugTC { get; set; }

	TextC debug { get; set; }

	Vector3 position { get; }

	Camera camera { get; set; }

	ConnectionSlot[] inputSlots { get; set; }

	ConnectionSlot[] modifierSlots { get; set; }

	ConnectionSlot[] outputSlots { get; set; }

	bool update { get; set; }

	int collidingCount { get; set; }

	int triggerCount { get; set; }

	bool autoTrigger { get; set; }

	bool triggered { get; set; }

	bool began { get; set; }

	bool end { get; set; }

	float beganTime { get; set; }

	float endTime { get; set; }

	bool toggle { get; set; }

	bool triggerOnlyOnce { get; set; }

	bool triggerOnlyOnFullEnergy { get; set; }

	bool triggerUntilOutOfEnergy { get; set; }

	int energyClips { get; set; }

	float triggerCooldown { get; set; }

	float reloadCooldown { get; set; }

	float gainCooldown { get; set; }

	float lastReload { get; set; }

	bool reloading { get; set; }

	float energy { get; set; }

	float energyConsume { get; set; }

	float energyGain { get; set; }

	float energyConsumeInterval { get; set; }

	float energyGainInterval { get; set; }

	float lastConsume { get; set; }

	float lastGain { get; set; }

	GEControlledValue input { get; set; }

	GEControlledValue output { get; set; }

	GEControlledValue modifier { get; set; }

	ModifierType modifierType { get; set; }

	int beganDelegatedCount { get; set; }

	TriggerEventDelegate BeganEventDelegate { get; set; }

	int endDelegatedCount { get; set; }

	TriggerEventDelegate EndEventDelegate { get; set; }
}
