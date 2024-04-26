using System.Windows;
using System.Windows.Controls;

public class CustomTextBlock : TextBlock
{
	public static readonly DependencyProperty Text1Property = DependencyProperty.Register("Text1", typeof(string), typeof(CustomTextBlock));
	public static readonly DependencyProperty Text2Property = DependencyProperty.Register("Text2", typeof(string), typeof(CustomTextBlock));
	public static readonly DependencyProperty Text3Property = DependencyProperty.Register("Text3", typeof(string), typeof(CustomTextBlock));
	public static readonly DependencyProperty Text4Property = DependencyProperty.Register("Text4", typeof(string), typeof(CustomTextBlock));

	public string Text1
	{
		get { return (string)GetValue(Text1Property); }
		set { SetValue(Text1Property, value); }
	}

	public string Text2
	{
		get { return (string)GetValue(Text2Property); }
		set { SetValue(Text2Property, value); }
	}

	public string Text3
	{
		get { return (string)GetValue(Text3Property); }
		set { SetValue(Text3Property, value); }
	}

	public string Text4
	{
		get { return (string)GetValue(Text4Property); }
		set { SetValue(Text4Property, value); }
	}
}
