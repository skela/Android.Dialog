using System;
using System.Drawing;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class EntryElement : Element
	{
		public string Value
		{
			get
			{
				return val;
			}
			set
			{
				val = value;
				if (entry != null)
					entry.Text  = value;
			}
		}
		
		private string val;
		private string hint;
		private bool isPassword;
		private EditText entry;
		private RelativeLayout cell;
		private TextView captionLabel;
		private Context _context = null;

		public event EventHandler Changed;

		private void Init(string value,string hint,bool isPassword=false,UIKeyboardType keyboardType=UIKeyboardType.ASCIICapable)
		{
			Value = value;
			this.hint = hint;
			this.isPassword = isPassword;
			KeyboardType = keyboardType;
		}

		public EntryElement(string caption, string hint, string value,UIKeyboardType keyboardType=UIKeyboardType.ASCIICapable) : base(caption)
		{
			Init (value,hint,false,keyboardType);
		}

		public EntryElement(string caption, string hint, string value,bool isPassword,UIKeyboardType keyboardType=UIKeyboardType.ASCIICapable) : base(caption)
		{
			Init (value,hint,isPassword,keyboardType);
		}

		public override string Summary()
		{
			return Value;
		}

		SizeF ComputeEntryPosition()
		{
			Section s = Parent as Section;
			if (s.EntryAlignment.Width != 0)
				return s.EntryAlignment;

			SizeF max = new SizeF(-1, -1);
			foreach (var e in s.Elements)
			{
				var ee = e as EntryElement;
				if (ee == null)
					continue;
			}
			s.EntryAlignment = new SizeF(25 + Math.Min(max.Width, 160), max.Height);
			return s.EntryAlignment;
		}

		public override View GetView(Context context, View convertView, ViewGroup parent)
		{
            _context = context;
			if (cell==null)
			{
				cell = new RelativeLayout(context);

				if (entry == null)
				{
					var _entry = new EditText(context)
									 {
										 Tag = 1,
										 Hint = hint ?? "",
										 Text = Value ?? "",
									 };

					_entry.ImeOptions = MonoDroidDialogEnumHelper.ImeActionFromUIReturnKeyType(ReturnKeyType);
					_entry.InputType = MonoDroidDialogEnumHelper.InputTypesFromUIKeyboardType(KeyboardType);

					if(isPassword)
					{
						_entry.InputType = Android.Text.InputTypes.TextVariationPassword;
					}

					entry = _entry;

					entry.TextChanged += delegate
					{
						FetchValue();
					};
				}
				var tvparams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
															  ViewGroup.LayoutParams.WrapContent);
				tvparams.SetMargins(5,3,5,0);
	            tvparams.AddRule(LayoutRules.CenterVertical);
	            tvparams.AddRule(LayoutRules.AlignParentLeft);

				captionLabel = new TextView(context) {Text = Caption, TextSize = 16f};

				var eparams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
															  ViewGroup.LayoutParams.WrapContent);
				eparams.SetMargins(5, 3, 5, 0);
	            eparams.AddRule(LayoutRules.CenterVertical);
	            eparams.AddRule(LayoutRules.AlignParentRight);

				cell.AddView(captionLabel,tvparams);
				cell.AddView(entry,eparams);
			}
			else
			{
				captionLabel.Text = Caption;
				entry.Text = Value;
			}
			return cell;
		}

		public void FetchValue()
		{
			if (entry == null)
				return;

			var newValue = entry.Text.ToString();
			var diff = newValue != Value;
			val = newValue;

			if (diff && Changed != null)
			{
				Changed(this, EventArgs.Empty);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				entry.Dispose();
				entry = null;

				captionLabel.Dispose ();
				captionLabel = null;

				cell.Dispose ();
				cell = null;
			}
		}

		public override bool Matches(string text)
		{
			return (Value != null ? Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) || base.Matches(text);
		}

		#region MonoTouch Dialog Mimicry
		
		public UIKeyboardType KeyboardType
		{
			get { return keyboardType; }
			set { keyboardType = value; }
		}
		private UIKeyboardType keyboardType;
		
		public UIReturnKeyType ReturnKeyType
		{
			get { return returnKeyType; }
			set { returnKeyType = value; }
		}
		private UIReturnKeyType returnKeyType;
		
		// Not used in any way, just there to match MT Dialog api.
		public UITextFieldViewMode ClearButtonMode
		{
			get { return clearButtonMode; }
			set { clearButtonMode = value; }
		}
		private UITextFieldViewMode clearButtonMode;
		
		#endregion
	}
}