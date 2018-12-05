using FaceDetectFormsDemo.iOS.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellRenderer))]
namespace FaceDetectFormsDemo.iOS.Renderers
{
    public class ViewCellRenderer : Xamarin.Forms.Platform.iOS.ViewCellRenderer
    {

        public override UIKit.UITableViewCell GetCell(Cell item, UIKit.UITableViewCell reusableCell, UIKit.UITableView tv)
        {

            var cell = base.GetCell(item, reusableCell, tv);
            cell.SelectionStyle = UIKit.UITableViewCellSelectionStyle.None;

            return cell;
        }

    }
}
