   M S B A M L   `   `   `  ����D  @AutoSkola, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:AutoSkola	AutoSkola  5   	       AutoSkola.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:AutoSkola     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � $��
0,0,-6.4,0q�6   .+�5
   
   &   System.Windows.Controls.DataGrid  -   5        $��dataGridCentralni��5
      $��Center=�6,   $��66,55,66.4,57q�6J   $��Center=�6a   $	��207��5         
IsReadOnly .    AlternatingRowBackground
 ��� �   CanUserAddRows .     SelectionUnit$ FullRow=�6i      SelectionMode$ Single=�6�   $	��616��6�   �� 5   
   $��Center=�6   $��30��62   $	��Top=�6?   $	��614��6W    �� Orientation$ 
Horizontal=�6c   .+�5      �� -   5        $��btnkandidat��5      $��Kandidat��6(   $
��Left=�6;   $��90��6V   $��	0,0,0,0.4q�6b   6u   �� 5      -   5        $��	btnIspiti��5      $��Ispiti��6&   $
��Left=�67   $	��Top=�6S   $��30��6k   $��75��6w   6�   �� 5      -   5        $��btnLekarskiPregled��5      $��Lekarski Pregled��6/   $
��Left=�6J   $	��Top=�6f   $��30��6~   $	��105��6�   6�   �� 5      -   5        $��btnPrvaPomoc��5      $��Prva Pomoć��6)   $
��Left=�6>   $	��Top=�6Z   $��30��6r   $��75��6~   6�   �� 5      -   5        $��btnInstruktor��5      $��
Instruktor��6*   $
��Left=�6?   $	��Top=�6[   $��30��6s   $��75��6   6�   �� 5      -   5        $��btnZaposleni��5      $��	Zaposleni��6)   $
��Left=�6=   $	��Top=�6Y   $��30��6q   $��75��6}   6�   �� 5      -   5        $��
btnPotvrda��5      $��Potvrda��6'   $	��Top=�69   $��30��6Q   $��64��6]   6h   �� 5      -	   5        $��btnObuka��5      $��Obuka��6&   $��55��66   6A   5      �� 5   
   $��Center=�6   $��30��62   $��Bottom=�6?   $
��auto��6Z   $ 
Horizontal=�6g   .+�5      �� -
   5        $��btnDodaj��5      $��Dodaj��6%   $	��120��65   6A   �� 5      -   5        $��	btnIzmeni��5      $��Izmeni��6&   $	��120��67   6C   �� 5      -   5        $��	btnObrisi��5      $��Obrisi��6&   $	��120��67   6C   5      5      5      