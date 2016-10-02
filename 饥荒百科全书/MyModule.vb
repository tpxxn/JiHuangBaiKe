Module MyModule

    Public PanelAlpha As Single = 0.6

    REM 食物全局变量
    Public F_Ingredients_1 As String = ""
    Public F_Ingredients_2 As String = ""
    Public F_FoodNeed_1 As String = "FC_Meats"
    Public F_FoodNeed_or As String = ""
    Public F_FoodNeed_2 As String = "FC_Eggs"
    Public F_FoodNeed_3 As String = ""
    Public F_Restrictions_1 As String = "FC_Vegetables"
    Public F_Restrictions_2 As String = ""
    Public F_Restrictions_3 As String = ""
    Public F_Restrictions_4 As String = ""
    Public F_Restrictions_5 As String = ""
    Public F_Restrictions_6 As String = ""
    Public F_Restrictions_7 As String = ""
    Public F_Restrictions_Compare As String = ""

    Public F_Recommend_Select_1 As String = "F_meat"
    Public F_Recommend_Select_2 As String = "F_morsel"
    Public F_Recommend_Select_3 As String = "F_egg"
    Public F_Recommend_Select_4 As String = "F_egg"
    Public F_Recommend_Select_5 As String
    Public F_Recommend_Select_6 As String
    Public F_Recommend_Select_7 As String
    Public F_Recommend_Select_8 As String

    REM 科技全局变量
    Public S_ScienceNeed_1 As String = "G_twigs"
    Public S_ScienceNeed_2 As String = "G_flint"
    Public S_ScienceNeed_3 As String

    REM 烹饪模拟全局变量
    Public CS_PortableCrockPot As Boolean = True
    '四个位置
    Public CS_Recipe_1 As String
    Public CS_Recipe_2 As String
    Public CS_Recipe_3 As String
    Public CS_Recipe_4 As String
    '37种食材
    Public CS_FT_Banana As Single = 0
    Public CS_FT_Berries As Single = 0
    Public CS_FT_Butter As Single = 0
    Public CS_FT_Butterfly_wings As Single = 0
    Public CS_FT_CactusFlesh As Single = 0
    Public CS_FT_CactusFlower As Single = 0
    Public CS_FT_Corn As Single = 0
    Public CS_FT_DairyProduct As Single = 0
    Public CS_FT_DragonFruit As Single = 0
    Public CS_FT_Drumstick As Single = 0
    Public CS_FT_Eel As Single = 0
    Public CS_FT_Eggplant As Single = 0
    Public CS_FT_Eggs As Single = 0
    Public CS_FT_Fishes As Single = 0
    Public CS_FT_FrogLegs As Single = 0
    Public CS_FT_Fruit As Single = 0
    Public CS_FT_Honey As Single = 0
    Public CS_FT_Ice As Single = 0
    Public CS_FT_Jellyfish As Single = 0
    Public CS_FT_Lichen As Single = 0
    Public CS_FT_Limpets As Single = 0
    Public CS_FT_Mandrake As Single = 0
    Public CS_FT_Meats As Single = 0
    Public CS_FT_Moleworm As Single = 0
    Public CS_FT_MonsterFoods As Single = 0
    Public CS_FT_Mussel As Single = 0
    Public CS_FT_Pumpkin As Single = 0
    Public CS_FT_RoastedBirchnut As Single = 0
    Public CS_FT_RoastedCoffeeBeans As Single = 0
    Public CS_FT_Seaweed As Single = 0
    Public CS_FT_SharkFin As Single = 0
    Public CS_FT_Sweetener As Single = 0
    Public CS_FT_SweetPotato As Single = 0
    Public CS_FT_Twigs As Single = 0
    Public CS_FT_Vegetables As Single = 0
    Public CS_FT_Watermelon As Single = 0
    Public CS_FT_Wobster As Single = 0

    Public FoodIndex As Byte = 0

    Public CS_ROG_SW_DST As SByte = 7 'DLC
    Public CS_F_name As String = "" '食物名称

    Public CrockPotList() As String '食物列表
    Public CrockPotListIndex As SByte = -1 '食物列表下标
    Public CrockPotMaxPriority As SByte = -128 '优先度最大值

    REM 生物全局变量
    Public A_LootP_Select_1 As String = "F_morsel"
    Public A_LootP_Select_2 As String = "A_rabbit"
    Public A_LootP_Select_3 As String
    Public A_LootP_Select_4 As String
    Public A_LootP_Select_5 As String
    Public A_LootP_Select_6 As String
    Public A_LootP_Select_7 As String
    Public A_LootP_Select_8 As String
    Public A_LootP_Select_9 As String
    Public A_LootP_Select_10 As String
    Public A_LootP_Select_11 As String
    Public A_LootTrapOrBugNetP_Select_1 As String
    Public A_LootTrapOrBugNetP_Select_2 As String = "S_trap"
    Public A_LootTrapOrBugNetP_Select_3 As String
    Public A_LootTrapOrBugNetP_Select_4 As String
    Public A_LootTrapOrBugNetP_Select_5 As String

    REM 自然群落全局变量
    Public NB_AbundantArray(7) As String
    Public NB_OccasionalArray(13) As String
    Public NB_RareArray(6) As String
    Public NP_ArrayIndex As Byte
    Public NP_PictureArray() As String
    Public NP_ResourcesArray(,) As String
    Public NP_ResourcesBurnt_1 As String
    Public NP_ResourcesBurnt_2 As String
    Public NP_ResourcesBurnt_3 As String
    Public NP_ResourcesBurnt_4 As String
    Public NP_SpecialAbility_1 As String
    Public NP_SpecialAbility_2 As String
    Public NP_Biome_1 As String
    Public NP_Biome_2 As String

    REM 物品全局变量
    Public GM_ScienceArray(40) As String
    Public GM_AnimalArray(28) As String
    Public GS_Equipment_FromAnimal_Select_1 As String
    Public GS_Equipment_FromAnimal_Select_2 As String
    Public GS_Sapling_Select_1 As String
    Public GS_Sapling_Select_2 As String
    Public GS_Turf_Select As String
    Public GP_Pet_Select_1 As String
    Public GP_Pet_Select_2 As String
    Public GP_Pet_Select_3 As String
    Public G_PetList() As String
    Public G_PetListIndex As SByte = 0
    Public G_PetListIndexMax As SByte = -128
    Public GS_UnlockCharacter_Select As String
    Public GS_UnlockDrop_Select_1 As String
    Public GS_UnlockDrop_Select_2 As String
    Public GS_UnlockDrop_Select_3 As String = "A_spitter"
    Public GS_UnlockDrop_Select_4 As String = "A_cave_spider"
    Public GS_UnlockDrop_Select_5 As String = "A_dangling_depth_dweller"
    Public GS_UnlockDrop_Select_6 As String = "A_spider_queen"
    Public GS_UnlockDrop_Select_7 As String = "N_spider_den"
    Public GS_UnlockDrop_Select_8 As String = "N_spilagmite"
    Public G_ComponentList() As String
    Public G_ComponentListIndex As SByte = 0
    Public G_ComponentListIndexMax As SByte = -128


    REM ------------------注册表读写函数------------------
    Public Sub Reg_Write(ValueName As String, Value As Integer)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\SOFTWARE\JiHuangBaiKe", ValueName, Value)
    End Sub

    Public Sub Reg_Write_string(ValueName As String, Value As String)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\SOFTWARE\JiHuangBaiKe", ValueName, Value)
    End Sub

    Public Function Reg_Read(ValueName As String)
        Dim GetValue As Integer
        Dim GetValueTemp As String
        GetValueTemp = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\JiHuangBaiKe", ValueName, String.Empty)
        If GetValueTemp <> "" Then
            GetValue = CInt(GetValueTemp)
        End If
        Return GetValue
    End Function

    Public Function Reg_Read_string(ValueName As String)
        Dim GetValue As String
        Dim GetValueTemp As String
        GetValueTemp = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\JiHuangBaiKe", ValueName, String.Empty)
        If GetValueTemp <> "" Then
            GetValue = CStr(GetValueTemp)
        End If
        Return GetValue
    End Function

    REM ------根据字符串长度返回TextBlock高度------
    Public Function SetTextBlockHeight(Text As String, colLen As Integer)
        Dim TextBlockHeight As Double
        TextBlockHeight = ((Len(Text) - 1) \ colLen + 1) * 15.24
        Return TextBlockHeight
    End Function

    REM ------------------资源短名------------------
    Public Function Res_Short_Name(R_URL As String)
        R_URL = "Resources/" & R_URL & ".png"
        Return R_URL
    End Function

    REM ------------------图片短名------------------
    Public Function Picture_Short_Name(Optional source As String = "")
        Dim Picture As New BitmapImage
        Picture.BeginInit()
        If source = "" Then
            Picture.UriSource = New Uri("{x:Null}", UriKind.Relative)
        Else
            Picture.UriSource = New Uri(source, UriKind.Relative)
        End If
        Picture.EndInit()
        Return Picture
    End Function

End Module
