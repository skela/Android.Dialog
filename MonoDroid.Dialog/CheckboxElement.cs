using System;

using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class CheckboxElement : Element
    {
        private CheckBox checkbox;
		private RelativeLayout cell;
		private TextView captionLabel;

        public string Group;

		public bool Value
		{
			get { return val; }
			set
			{
				bool emit = val != value;
				val = value;
				if (checkbox != null && checkbox.Checked != val)
					checkbox.Checked = val;
				else if (emit && Changed != null)
					Changed(this, EventArgs.Empty);
			}
		}
		private bool val;

		public event EventHandler Changed;

        public CheckboxElement(string caption)
            : base(caption)
        {
        }

        public CheckboxElement(string caption, bool value)
            : this(caption)
        {
            Value = value;
        }

        public CheckboxElement(string caption, bool value, string group)
            : this(caption, value)
        {
            Group = group;
        }

		public override View GetView(Context context, View convertView, ViewGroup parent)
        {
			if (cell==null)
			{
				cell = new RelativeLayout(context);

				if (checkbox==null)
				{
					var _cb = new CheckBox(context);
					_cb.Checked = Value;

					checkbox = _cb;
					checkbox.CheckedChange+= delegate(object sender, CompoundButton.CheckedChangeEventArgs e) 
					{
						FetchValue();
					};
				}
				var tvparams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,ViewGroup.LayoutParams.WrapContent);
				tvparams.SetMargins(5,3,5,0);
				tvparams.AddRule(LayoutRules.CenterVertical);
				tvparams.AddRule(LayoutRules.AlignParentLeft);
				
				captionLabel = new TextView(context) {Text = Caption, TextSize = 16f};
				
				var eparams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,ViewGroup.LayoutParams.WrapContent);
				eparams.SetMargins(5, 3, 5, 0);
				eparams.AddRule(LayoutRules.CenterVertical);
				eparams.AddRule(LayoutRules.AlignParentRight);

				cell.AddView(captionLabel,tvparams);
				cell.AddView(checkbox,eparams);
			}
			else
			{
				captionLabel.Text = Caption;
				checkbox.Checked = Value;
			}
			return cell;
        }

		public void FetchValue()
		{
			if (checkbox == null)
				return;
			
			var newValue = checkbox.Checked;
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
				checkbox.Dispose();
				checkbox = null;

				cell.Dispose ();
				cell = null;
			}
		}

		//public override void Selected()
		//{
		//    Value = !Value;
		//    _cb.Checked = Value;
		//}
    }
}