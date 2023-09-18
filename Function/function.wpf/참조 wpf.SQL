줄바꿈 &#10; <LineBreak />
<TextBlock xml:space="preserve">


GridMain="{Binding ElementName=MainGrid}"
<RowDefinition Height="{Binding Source={x:Static info:vari.pageHeaderHeight}}" />


{StaticResource {x:Static SystemColors.ControlColorKey}}

<wpf:baseWindow 
  xmlns:wpf="clr-namespace:function.wpf;assembly=function.wpf" 
	CloseWindowToType="PromptYN" SavePosition="True" isUseFormInit="True">

StatusBar = STBar;
<wpf:wucStatusBar x:Name="STBar"   Grid.Row="1" Grid.RowSpan="2" Background="LightGray"/>

protected override void OnPreviewKeyDown(KeyEventArgs e)
{
	switch (e.Key)
	{	
		case Key.Back:
		if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)





  sBackColor = Fnc.Brush2String(brErr);

  function.wpf.wpfMsgBox f = new function.wpf.wpfMsgBox("종료", "종료", false);
  f.ShowDialog(this);

  vari.path = AppDomain.CurrentDomain.BaseDirectory;

  //바인딩 
  <TextBox x:Name="txtText" Text="{Binding SelectedItem[Text], ElementName=dgLeft}"
                 Grid.Column="1" HorizontalAlignment="Left" Height="28" Margin="20,77,0,0" TextWrapping="Wrap"  VerticalAlignment="Top" Width="198"/>


  //데이터그리드 DragDrop
  function.wpf.DataGridItemMove dm = new DataGridItemMove();
  dm.DataGridAdd(dgRight, "No");  //no 컬럼 기준으로 정렬 처리

  //TreeView DragDrop
  function.wpf.TreeViewItemMove tm = new function.wpf.TreeViewItemMove(treeView2);


public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(wucInputBox),
new FrameworkPropertyMetadata((object)null, ValueChangeCallback));

/// <summary>
/// 원본 값
///
///</summary>
public string Value
{
  get { return GetValue(ValueProperty) == null ? null  : (string)GetValue(ValueProperty); }
  set { SetValue(ValueProperty, value); }
}

static void ValueChangeCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
{
  wucInputBox w = o as wucInputBox;
  w.valueChaged();
}



  <DataGrid Name="grdMC" Grid.Row="1" Grid.ColumnSpan="3" AutoGenerateColumns="False" CanUserAddRows="False" CanUserDeleteRows="False" MouseDoubleClick="grdMC_MouseDoubleClick" >
  <DataGrid.Resources>
                <Style TargetType="DataGridColumnHeader">
                    <Setter Property="HorizontalContentAlignment" Value="Center"/>
                </Style>
            </DataGrid.Resources>
  <DataGrid.Columns>
    <DataGridTextColumn Header="부서명" Width="90" Binding="{Binding DEPT_GBN_NM}" IsReadOnly="True">
		<DataGridTextColumn.ElementStyle>
            <Style TargetType="TextBlock">
                <Setter Property="HorizontalAlignment" Value="Left" />
            </Style>
        </DataGridTextColumn.ElementStyle>
	<DataGridTextColumn>

	<DataGridTextColumn Header="바코드번호" Width="250" Binding="{Binding ID_NO}" IsReadOnly="True" ElementStyle="{StaticResource ResourceKey=tbCenter}">
		 <DataGridTextColumn.CellStyle>		//셀색만 변경
			<Style TargetType="DataGridCell">
				<Setter Property="Background" Value="{StaticResource ResourceKey=brhChangable}" />
				<EventSetter Event="PreviewMouseDown" Handler="dgItemCell_PreviewMouseDown" />		<!-- private void dgItemCell_PreviewMouseDown(object sender, MouseButtonEventArgs e) -->
			</Style>
		</DataGridTextColumn.CellStyle>
	</DataGridTextColumn>

  </DataGrid.Columns>
  <DataGrid.RowStyle>
    <Style TargetType="DataGridRow">
      <Setter Property="Background" Value="{Binding BackColor}"/>
      <Setter Property="Foreground" Value="{Binding ForeColor}"/>
	  <Setter Property="Height" Value="55"/>
    </Style>
  </DataGrid.RowStyle>
</DataGrid>


  <ListView Name="grdMC" Grid.Row="1" Grid.ColumnSpan="3" AutoGenerateColumns="False" CanUserAddRows="False" CanUserDeleteRows="False" MouseDoubleClick="grdMC_MouseDoubleClick" >
    <ListView.View>
      <GridView>
        <GridViewColumn Header="Date" DisplayMemberBinding="{Binding Name}" Width="{Binding Path=ActualWidth, ElementName=grdMC}" />
      </GridView>
    </ListView.View>

      <ListView.Resources>
        <Style TargetType="{x:Typew ListViewItem}">
          <Setter Property="Background" Value="{Binding BackColor}"/>
          <Setter Property="Foreground" Value="{Binding ForeColor}"/>
          <Style.Triggers>
            <DataTrigger Binding="{Binding Harf}" Value="1">
              <Setter Property="Background" Value="Red" />
            </DataTrigger>
          </Style.Trigger>          
        </Style>
    </ListView.Resources>
  </ListView>


  --listview 에서 헤더 숨김
<ListView.Resources>
	<Style TargetType="GridViewColumnHeader">
		<Setter Property="Visibility" Value="Hidden" />
	</Style>
</ListView.Resources>