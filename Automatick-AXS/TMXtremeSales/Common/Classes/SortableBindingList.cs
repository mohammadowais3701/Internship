#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Linq.Expressions;
#endregion

namespace SortedBindingList
{

    [Serializable]
    public class SortableBindingList<T> : BindingList<T>
    {

        public delegate void dlgOnChange(T item);
        public dlgOnChange changeDelegate;
        #region Sorting

        // reference to the list provided at the time of instantiation
        //List<T> originalList;

        ListSortDirection sortDirection;

        //PropertyDescriptor sortProperty;

        // function that refereshes the contents of the base classes collection of elements
        Action<SortableBindingList<T>, List<T>> populateBaseList = (a, b) => a.ResetItems(b);

        // a cache of functions that perform the sorting for a given type, property, and sort direction
        static Dictionary<string, Func<List<T>, IEnumerable<T>>> cachedOrderByExpressions =
            new Dictionary<string, Func<List<T>, IEnumerable<T>>>();

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            /*
             Look for an appropriate sort method in the cache if not found .
             Call CreateOrderByMethod to create one. 
             Apply it to the original list.
             Notify any bound controls that the sort has been applied.
             */

            PropertyDescriptor sortProperty;
            sortProperty = prop;


            var orderByMethodName = sortDirection == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending";

            var cacheKey = typeof(T).GUID + prop.Name + orderByMethodName;

            if (!cachedOrderByExpressions.ContainsKey(cacheKey))
            {
                CreateOrderByMethod(prop, orderByMethodName, cacheKey);
            }

            ResetItems(cachedOrderByExpressions[cacheKey](base.Items.ToList()).ToList());

            ResetBindings();

            sortDirection = sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending :
                                                                           ListSortDirection.Ascending;

        }

        private void CreateOrderByMethod(PropertyDescriptor prop, string orderByMethodName, string cacheKey)
        {
            /*
             Create a generic method implementation for IEnumerable<T>.
             Cache it.
            */
            var sourceParameter = Expression.Parameter(typeof(List<T>), "source");

            var lambdaParameter = Expression.Parameter(typeof(T), "lambdaParameter");

            var accesedMember = typeof(T).GetProperty(prop.Name);

            var propertySelectorLambda =
                Expression.Lambda(Expression.MakeMemberAccess(lambdaParameter, accesedMember), lambdaParameter);

            var orderByMethod = typeof(Enumerable).GetMethods()
                                                  .Where(a => a.Name == orderByMethodName &&
                                                               a.GetParameters().Length == 2)
                                                  .Single()
                                                  .MakeGenericMethod(typeof(T), prop.PropertyType);

            var orderByExpression = Expression.Lambda<Func<List<T>, IEnumerable<T>>>(
                                        Expression.Call(orderByMethod,
                                                        new Expression[] { sourceParameter, 
                                                                           propertySelectorLambda }),
                                                        sourceParameter);

            cachedOrderByExpressions.Add(cacheKey, orderByExpression.Compile());
            //this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            ResetItems(base.Items.ToList());
        }

        public void RemoveSort()
        {
            RemoveSortCore();
        }
        private void ResetItems(List<T> items)
        {

            base.ClearItems();

            for (int i = 0; i < items.Count; i++)
            {
                base.InsertItem(i, items[i]);
            }

        }

        public void Sort(IOrderedEnumerable<T> orderList)
        {
            try
            {
                List<T> items = orderList.ToList();
                base.ClearItems();

                for (int i = 0; i < items.Count; i++)
                {
                    base.InsertItem(i, items[i]);
                }
            }
            catch (Exception)
            {

            }
        }
        protected override bool SupportsSortingCore
        {
            get
            {
                // indeed we do
                return true;
            }
        }

        //protected override ListSortDirection SortDirectionCore
        //{
        //    get
        //    {
        //        return SortDirectionCore;
        //    }
        //}

        //protected override PropertyDescriptor SortPropertyCore
        //{
        //    get { return SortPropertyCore; }
        //}
        //protected override PropertyDescriptor SortPropertyCore
        //{
        //    get
        //    {
        //        return sortProperty;
        //    }
        //}

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            //base.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));

            //originalList = base.Items.ToList();
            //e = new ListChangedEventArgs(e.ListChangedType);
            try
            {
                base.OnListChanged(e);
                //base.Items.ToList();
            }
            catch { }
        }

        #endregion

        #region Persistence

        // NOTE: BindingList<T> is not serializable but List<T> is

        public void Save(string filename)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, (List<T>)this.Items);
            }
        }

        public void Load(string filename)
        {

            this.ClearItems();

            if (File.Exists(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(filename, FileMode.Open))
                {
                    // Deserialize data list items
                    ((List<T>)this.Items).AddRange((IEnumerable<T>)formatter.Deserialize(stream));
                }
            }

            // Let bound controls know they should refresh their views
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        #endregion
    }
}
