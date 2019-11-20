# AZNPano
## Sprachliche Anforderung
- Jede Schicht ist von einem bestimmten **Typ**. Die Typen sind Frühschicht, Spätschicht, Mitternachtssauna und Special.

- Jede Schicht hat einen zeitlichen Anfang und ein zeitliches Ende. Dabei können Schichten auch über 0 Uhr gehen.

- An einem Tag können bis zu zwei Schichten absolviert werden. (Die Mitternachtssauna geht bis 3 Uhr und um 13:15 Uhr beginnt die Spätschicht).

- Der Grundlohn ist für jeden Schicht-Typ identisch. Jedoch gibt es je nach Schicht-Typ unterschiedliche Zusatzleistungen.

- Zu jeder Schicht soll notiert werden, in welchem Bereich gearbeitet wurde (Schwimmhalle oder Freibad).

- Ab 6 Stunden Arbeitszeit ist eine Pause Pflicht. Eine Pause hat einen zeitlichen Anfang und ein zeitliches Ende. Eine Pause kann zusätzlich von zwei Typen sein (Normal oder Bereitschaft).

## E/R-Diagramm

![ERhdff](Unterlagen/ERDiagramm.png)

## Relationenschreibweise

- Typ(**T_ID**, Bezeichnung)
- Bereich(**B_ID**, Bezeichnung)
- Pause(**P_ID**, Art, Beginn, Ende)
- Schicht(**S_ID**, _T_ID_, _B_ID_, _P_ID_, Beginn, Ende)
- Gehalt(**G_ID**, Betrag)
- SchichtGehalt(**S_ID**, **G_ID**)
