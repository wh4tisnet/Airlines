   M S B A M L   `   `   `  ����C  ?Airlines, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:AirlinesAirlines  5   	       Airlines.bs   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Airlines     5   	   -   5          � Title$  bs��5   	   $	��450��6   $	��800��6!   .��5	      � .+�5
   
   �� $��Plane Company:��6   $
��Left=�6*   $	��Top=�6E   $��
73,278,0,0q�6]   $��14(�6q   
���   �6   �� 5   
   -   5        $��cboPassengerSelection��5      $
��Left=�60   $	��Top=�6K   $��224,278,0,0q�6c   $	��150��6x    �� DisplayMemberPath$ Value��6�    �� SelectedValuePath$	 Key��6�   �� 5   
   $��State From:��6   $
��Left=�6'   $	��Top=�6B   $��
73,343,0,0q�6Z   $��14(�6n   
���   �6|   �� 5   
   -   5         ��Name$ 
cboCountry��5      $
��Left=�6'   $	��150��6B   $��224,342,0,0q�6N   $ Value��6�   $	 Key��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   $��22��6�   $	��Top=�6�   �� 5   
   $��	State To:��6   $
��Left=�6%   $	��Top=�6@   $��
73,309,0,0q�6X   $��14(�6l   
���   �6z   �� 5   
   -   5        $ cboCountryIs��5      $
��Left=�6)   $	��150��6D   $��224,310,0,0q�6P   $ Value��6�   $	 Key��6�   $ 0.5,0.5&�6�   $��22��6�   $	��Top=�6�   z�5      V� 5      .~�5      �� �� 5       �� AngleY$ -0.381Q�6$   �� 5       �� Angle$ 0.364Q�6&   T� 5       T� X$ 0.003Q�6)    T� Y$ -0.498Q�63   5      5      &   System.Windows.Controls.DataGrid  5   
   -   5        $��dtgPassengers��5      	   AutoGenerateColumns	 .  $��46,32,288,185q�6D   
   Columns
 5      4  .System.Windows.Controls.DataGridComboBoxColumn  5      -   5        cbogrdplaneCompany��,  &System.Windows.Controls.DataGridColumn   Header$ Plane Company��5   E      SelectedValueBinding 6\   ��  �� Path$ planeCompany� �� Mode$ TwoWay=�  5      -   5        
cbogrdFrom��$ 
State From��5   =    6Q   �� $ 	stateFrom�  5      -   5        cbogrdTo��$ State To��5   ;    6M   �� $ stateTo�0  *System.Windows.Controls.DataGridTextColumn  5      $ Arrival Date Time��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6@   �� $ arrivalDateTime� �� StringFormat$ D��  5      $ Departure Date Time��6%    6B   �� $ departureDateTime�$ D��  5       $
 Name��6%    63   �� $
 name�  5!      $ Contact Email��6%    6<   �� $ contactEmail�  5"      $ Contact Telephone��6%    6@   �� $ contactTelephone�4  .System.Windows.Controls.DataGridCheckBoxColumn  5#      $ Business Class��6)    6A   �� $ isBusinessClass�5%      �� 5&   
   -	   5        $��Update Table��5&      $
��Left=�6(   $	��Top=�6C   $��591,187,0,0q�6[   $	��150��6p   $��30��6|   
�������6�   �� 5'   
   -
   5        $��Delete User��5'      $
��Left=�6'   $	��Top=�6B   $��591,111,0,0q�6Z   $	��150��6o   $��30��6{   
����+��6�   5(      5)      