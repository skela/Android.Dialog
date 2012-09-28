using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
	public class DialogAdapter : BaseAdapter<Section>
	{
		const int TYPE_SECTION_HEADER = 0;

		Context context;
		//LayoutInflater inflater;

		public DialogAdapter(Context context, RootElement root,ListView listView=null)
		{
			this.context = context;
			//this.inflater = LayoutInflater.From(context);
			this.Root = root;

			// This is only really required when using a DialogAdapter with a ListView, in a non DialogActivity based activity.
			if (listView!=null)
				listView.ItemClick += ListView_ItemClick;
		}

		public RootElement Root
		{
			get;
			set;
		}

		public override bool IsEnabled(int position)
		{
			// start counting from here
			int typeOffset = TYPE_SECTION_HEADER + 1;

			foreach (var s in Root.Sections)
			{
				if (position == 0)
					return false;

				int size = s.Adapter.Count + 1;

				if (position < size)
					return true;

				position -= size;
				typeOffset += s.Adapter.ViewTypeCount;
			}

			return false;
		}

		public override int Count
		{
			get
			{
				int count = 0;

				//Get each adapter's count + 1 for the header
				foreach (var s in Root.Sections)
					count += s.Adapter.Count + 1;

				Console.WriteLine("Count is " + count);

				return count;
			}
		}

		public override int ViewTypeCount
		{
			get
			{
				/*
				//The headers count as a view type too
				int viewTypeCount = 1;

				//Get each adapter's ViewTypeCount
				foreach (var s in Root.Sections)
					viewTypeCount += s.Adapter.ViewTypeCount;

				Console.WriteLine("ViewTypeCount is " + viewTypeCount);

				return viewTypeCount;*/
				return Count;
			}
		}

		public override Section this[int position]
		{
			get { return this.Root.Sections[position]; }
		}

		public override bool AreAllItemsEnabled()
		{
			return false;
		}

		public override int GetItemViewType(int position)
		{
			// start counting from here
			int typeOffset = TYPE_SECTION_HEADER + 1;

			foreach (var s in Root.Sections)
			{
				if (position == 0)
					return (TYPE_SECTION_HEADER);

				int size = s.Adapter.Count + 1;

				if (position < size)
					return (typeOffset + s.Adapter.GetItemViewType(position - 1));

				position -= size;
				typeOffset += s.Adapter.ViewTypeCount;
			}

			return -1;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		//List<View>viewCache = new List<View>();

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			//View v=null;
			//int origPos = position;
			//if (position<viewCache.Count)
			//	return viewCache[position];

			int sectionIndex = 0;
			foreach (var s in Root.Sections)
			{
				if (s.Adapter.Context == null)
					s.Adapter.Context = this.context;

				if (position == 0)
				{
					//v=s.GetView(context, convertView, parent);
					//break;
					return s.GetView(context, convertView, parent);
				}
				int size = s.Adapter.Count + 1;

				if (position < size)
				{
					//v=(s.Adapter.GetView(position - 1, convertView, parent));
					//break;
					return (s.Adapter.GetView(position - 1, convertView, parent));
				}
				position -= size;
				sectionIndex++;
			}
			//if (v!=null)
			//	viewCache.Insert(origPos,v);
			//return v;
			return null;
		}

		/// <summary>
		/// Return the Element for the flattened/dereferenced position value.
		/// </summary>
		/// <param name="position">The direct index to the Element.</param>
		/// <returns>The Element object at the specified position or null if too out of bounds.</returns>
		public Element ElementAtIndex(int position)
		{
			int sectionIndex = 0;
			foreach (var s in Root.Sections)
			{
				if (position == 0)
					return Root.Sections[sectionIndex];
				
				// note: plus two for the section header and footer views
				int size = s.Adapter.Count + 1;
				if (position < size)
					return Root.Sections[sectionIndex][position - 1];
				position -= size;
				sectionIndex++;
			}
			
			return null;
		}

		/// <summary>
		/// Handles the ItemClick event of the ListView control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="Android.Widget.AdapterView.ItemClickEventArgs"/> instance containing the event data.</param>
		public void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var elem = ElementAtIndex(e.Position);
			if (elem == null) return;
			//elem.Selected();
			if (elem.Click != null)
				elem.Click();
		}
		
		/// <summary>
		/// Handles the ItemLongClick event of the ListView control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="Android.Widget.AdapterView.ItemLongClickEventArgs"/> instance containing the event data.</param>
		public void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
		{
			var elem = ElementAtIndex(e.Position);
			if (elem != null && elem.LongClick != null)
				elem.LongClick();
		}
	}
}