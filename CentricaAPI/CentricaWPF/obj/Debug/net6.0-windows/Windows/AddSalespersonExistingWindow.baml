   M S B A M L   `   `   `  ????F  BCentricaWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:CentricaWPF.WindowsCentricaWPF.Windows  5   	   6    0CentricaWPF.Windows.AddSalespersonExistingWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:CentricaWPF.Windows     5   	     ? Title$"  AddSalespersonExistingWindow??5   	   $	??425??6.   $	??350??6;    ? 
ResizeMode$ NoResize=?5	   	   .??5
      ?? $??15q?6   .+?5   
   ?? 
???????
???????$
??Bold"?6:   $??22(?6L   .??6Z   Available salespersons6r   ?? 5   
   -   5        $??ListAvailableSalespersons??5      $	??216??64   $??0,0,0,10q?6A    ?? View 5      ?? 5      .P?5      ?? $??Family name??6%    ?? Width$	 150??5       ?? DisplayMemberBinding 5   <   ?? *
FamilyName+5      ?? 5      $??
First name??6%   $	 150??5       5   ;   ?? *	FirstName+5      5      5      ?? 5   
   -   5        $??CheckBoxIsPrimary??5      $??18(?6,   $??0,0,0,10q?6:   .??6L   	Primary6U   ? 5   
   $??50??6    ? ColumnDefinitions 5      ?? 5      $
??0.1* ?6#   ?? 5      $
??1.5* ?6#   ?? 5      $
??0.1* ?6#   ?? 5      $??1* ?6#   ?? 5      $
??0.1* ?6#    ? RowDefinitions 5       ?? 5!      $
??0.2* ?6    ?? 5"      $??1* ?6    ?? 5#      $
??0.2* ?6    .+?5%      ?? -   5        $??1??5%      $??1??6"   .??6J   Add6O   ?? 5&      -   5        $??1??5&      $??3??6"   .??6M   Cancel6U   5'      5(      5)      