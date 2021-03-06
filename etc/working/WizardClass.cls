VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "WizardClass"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Private Main As Object
'Private Main As ccWeb3.MainClass
'
'====================================================================================
'
'====================================================================================
'
Public Function Execute(CsvObj As Object, MainObj As Object, OptionString As String, FilterInput As String) As String
    Call Init(MainObj)
    Execute = GetContent(OptionString)
    Exit Function
    '
    ' ----- Error Trap
    '
ErrorTrap:
    Call HandleError("WizardClass", "Execute", Err.Number, Err.Source, Err.Description, True, False)
End Function
'
Public Sub Init(MainObject As Object)
        '
        Set Main = MainObject
        '
    Exit Sub
ErrorTrap:
    Call HandleError("WizardClass", "Init", Err.Number, Err.Source, Err.Description, True, False)
End Sub
'
'====================================================================================
'
'====================================================================================
'
Public Function GetContent(OptionString As String) As String
    On Error GoTo ErrorTrap
    '
    Dim Common As New CommonClass
    '
    If Not (Main Is Nothing) Then
        GetContent = Common.GetForm(Main)
    End If
    '
    Exit Function
ErrorTrap:
    Call HandleError("WizardClass", "GetContent", Err.Number, Err.Source, Err.Description, True, False)
    End Function


