## DNAXFCircleProgress


### Circular progress indicator for Xamarin Forms iOS & Android.

This control will allow you to create an animated donought's style graph, with text in the middle.
You can control background and foreground color, text color, minimun, maximun and current cricle value, text value.
In iOS you can also control the width of the circular bar. In android, as we are using layouts, you can't change directly from XAML the width, but in the layout files of the project.

![Circle Progress iOS](/screenshots/iOS.gif?raw=true "Circle Progress iOS")
![Circle Progress Android](/screenshots/android.gif?raw=true "Circle Progress android")

### Using the control

Simply add the projects to your solution and reference them both in PCL and Platform projects (as seen in the sample code).

In your XAML, add a xmlns pointing to DNAXFCircleProgress and you are ready to use the control:

'''
<controls:XFCircleProgress WidthRequest="300" HeightRequest="300"
                           VerticalOptions="CenterAndExpand" HorizontalOptions="CenterAndExpand"
                           Maximun="200" Minimun="0" Value="125"
                           BackColor="LightGray" ForeColor="Green" TextColor="DarkBlue"
                           BarHeight="30" AnimationDuration="1000"
                           Text="125" TextSize="60"/>
'''

Enjoy! 