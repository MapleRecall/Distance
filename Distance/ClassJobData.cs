﻿internal struct ClassJobData
{
	internal string Abbreviation;
	internal bool DefaultSelected;
	internal ClassJobSortCategory SortCategory;

	internal enum ClassJobSortCategory
	{
		Job_Tank,
		Job_Healer,
		Job_Melee,
		Job_Ranged,
		Job_Caster,
		Class,
		HandLand,
		Other,
	};
}
