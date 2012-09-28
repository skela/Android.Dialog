using System;

using Android.Text;
using Android.Views.InputMethods;

namespace MonoDroid.Dialog
{
	public enum UITableViewCellAccessory
	{
		DisclosureIndicator
	}
	
	public enum UITextFieldViewMode
	{
		WhileEditing
	}
	
	public enum UIKeyboardType
	{
		NumberPad,
		DecimalPad,
		ASCIICapable,
		EmailAddress,
		PhonePad,
		NamePhonePad,
		NumbersAndPunctuation
	}
	
	public enum UIReturnKeyType
	{
		Default,
		Go,
		Google,
		Join,
		Next,
		Route,
		Search,
		Send,
		Yahoo,
		Done
	}	

	public static class MonoDroidDialogEnumHelper
	{
		public static ImeAction ImeActionFromUIReturnKeyType(UIReturnKeyType returnKeyType)
		{
			switch (returnKeyType)
			{
				case UIReturnKeyType.Default: return ImeAction.Unspecified;
				case UIReturnKeyType.Done: return ImeAction.Done;
				case UIReturnKeyType.Go: return ImeAction.Go;
				case UIReturnKeyType.Next: return ImeAction.Next;
				case UIReturnKeyType.Search: return ImeAction.Search;
				case UIReturnKeyType.Send: return ImeAction.Send;
			}
			return ImeAction.Unspecified;
		}

		public static InputTypes InputTypesFromUIKeyboardType(UIKeyboardType keyboardType)
		{
			switch (keyboardType)
			{
				case UIKeyboardType.DecimalPad: return InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;
				case UIKeyboardType.NumberPad: return InputTypes.ClassNumber;
				case UIKeyboardType.PhonePad: return InputTypes.ClassPhone;
				case UIKeyboardType.NamePhonePad: return InputTypes.TextVariationPersonName | InputTypes.ClassText;
				case UIKeyboardType.ASCIICapable: return InputTypes.TextVariationVisiblePassword | InputTypes.ClassText;
				case UIKeyboardType.NumbersAndPunctuation: return InputTypes.TextVariationVisiblePassword | InputTypes.ClassText;
				case UIKeyboardType.EmailAddress: return InputTypes.TextVariationEmailAddress | InputTypes.ClassText;
			}
			return InputTypes.ClassText;
		}
	}
}
