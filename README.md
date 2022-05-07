<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
<HEAD>
	<META HTTP-EQUIV="CONTENT-TYPE" CONTENT="text/html; charset=windows-1252">		 
</HEAD>
<BODY LANG="en-GB" DIR="LTR">
<TABLE WIDTH=100% BORDER=0 CELLPADDING=4 CELLSPACING=0>
	<TR>
		<TD>
			<H2>ASCOM Switch Driver Fritzbox DECT (C#)</H2>
		</TD>
	</TR>
</TABLE>
<P><BR><BR>
</P>
<H4>Treiber fuer Fritzbox DECT schaltbare Steckdosen. Version 1.0 Stand:07.05.2022</H4>
<HR>
<P CLASS="note">Das ist mein erstes ASCOM-Treiber Projekt.<BR>
	<BR> Ziel war es meine Schaltbaren Steckdosen der Fritzbox ueber ASCOM verwenden zu koennen.
    <BR>Ausgangslage waren die Templates for Developer die man auf den ASCOM seiten herunterladen kann.
	<BR>Das ganze wurde mit INNO als Installer verpackt.<BR><BR>


</P>
<P CLASS="underline">Zum Verwenden folgende Reihenfolge beachten:</P>

<UL>
	<LI><P STYLE="margin-bottom: 0cm"><b>Installieren:</b> Die <A HREF= https://github.com/Chr1sw1e/ASCOM-FritzboxDECT/blob/a5f11516d88bf9ece17d6df0a0a44bad1d0f9b45/fb/Fritzbox%20DECT%20switch%20Setup.exe>Installationsdatei</A> herunterladen und ausfuehren</P>
	<LI>
		<P STYLE="margin-bottom: 0cm"><b>Config:</b> ASCOM Diagnostic ausfuehren und unter Device Type
			<FONT FACE="Lucida Console, Courier New, Courier, monospace "> <b> switch </b></FONT> auswaehlen und dann im
			<FONT FACE="Lucida Console, Courier New, Courier, monospace "> <b> ASCOM switch Chooser </b></FONT> den Eintrag
			<FONT FACE="Lucida Console, Courier New, Courier, monospace "> <b> Fritzbox DECT als Switch </b></FONT> waehlen.
		</P>
	<LI>
		<P STYLE="margin-bottom: 0cm"><b>Einstellen:</b>			
			<FONT FACE="Lucida Console, Courier New, Courier, monospace "> <b>Properties..</b></FONT> waehlen -> beim 1. Start erscheint eine Fehlermeldung das keine Schalter vorhanden oder konfiguruert sind -> mit <b>OK</b> weiter
		</P>
	<LI><P STYLE="margin-bottom: 0cm"><b>Neues Fenster:</b> nach nachdem OK gedrueckt wurde erscheint ein neues Fenster zur Konfiguration des Treibers.</P>
	<LI><P STYLE="margin-bottom: 0cm"><b>Fritzbox switch setup:</b> IP (Standard:"Fritz.Box" sonst in der Form 192.168.0.0), Username und Passwort eingeben (Einstellungen sind aus der Fritzbox zu entnehmen, ggf Benutzer dort erstellen) </P>
	<LI><P STYLE="margin-bottom: 0cm"><b>Steckdosen Einlesen:</b> mit druecken auf
	<FONT FACE="Lucida Console, Courier New, Courier, monospace "> <b> Geraete neu einlesen </b></FONT> werden die verfuegbaren Steckdosen eingelesen
</P>
	<LI><P STYLE="margin-bottom: 0cm"><b>Auswahl Steckdosen:</b>in der Liste muessen nun die zu verwendeten Steckdosen auswaehlt werden.(Haken an Checkbox)</P>
	<LI><P STYLE="margin-bottom: 0cm"><b>Speichern:</b>mit OK werden die ausgewaehlten Steckdosen im Profil gespeichert</P>
	<LI><P STYLE="margin-bottom: 0cm"><b>Connect:</b>nun koennen die Steckdosen im ASCOM verwendet und geschaltet werden.</P>
</UL>

<H3>Notes:</H3>
<UL>
	<LI><P STYLE="margin-bottom: 0cm">Ist der Treiber schon konfiguriert werden nur die vorher ausgewaehlten Steckdosen angezeigt. Will man andere auswaehlen muss erneut <FONT FACE="Lucida Console, Courier New, Courier, monospace "> <b> Geraete neu einlesen </b></FONT> gedrueckt und alle gewuenschten Steckdosen wieder neu ausgewaehlt werden.
	</P>
	<LI><P STYLE="margin-bottom: 0cm">Um den Traffic im Netzwerk gering zu halten wird der aktuelle Schaltzustand der Steckdosen nur beim ersten Verbinden abgefragt, d.h. sollten die Steckdosen ueber z.B. einer APP geschaltet werden bekommt der ASCOM teiber nichts davon mit und geht davon aus das der letzte Schaltzustand besteht. Man kann diesen geaenderten Schaltzustand einlesen indem man den Treiber trennt und neu verbindet oder man parallel dazu auch den Schaltzustand des Schalters im ASCOM aendert.
	</P>
	<LI><P>Ich habe den Treiber in Microsoft Visual Studio Community 2022 (64-Bit) mit c# mit Hilfe der ASCOM templates entwickelt und auf WINDOWS 10 -64bit Treiber mit ASCOM 6.6 - 6.6.0.3444 getestet. Ich habe bis jetzt keinerlei Probleme festgestellt, was aber nicht bedeutet das der Treiber auf anderen Systemen genauso laeuft.</P>
	<LI><P>	Wer mir dafuer einen Kaffe ausgeben will kann das gerne hier <BR><BR>
		</P>
													
<A HREF="https://www.paypal.com/donate/?hosted_button_id=FD783VJDDSGHG"> <IMG SRC="https://www.paypalobjects.com/en_US/i/btn/btn_donate_LG.gif" NAME="spenden" ></A>				
</UL>
<DIV ALIGN=RIGHT>
	<TABLE WIDTH=100% BORDER=0 CELLPADDING=4 CELLSPACING=0>
		<TR>
			<TD>
				<TABLE WIDTH=100% BORDER=0 CELLPADDING=4 CELLSPACING=0>
					<TR>
						<TD>
							<H3>ASCOM Initiative</H3>
						</TD>
						<TD>
							<IMG SRC="https://user-images.githubusercontent.com/91977433/167251787-31c4b5cb-8186-4177-9e27-5804976b95f7.png" NAME="graphics1" ALIGN=RIGHT WIDTH=48 HEIGHT=56 BORDER=0></TD>
					</TR>
				</TABLE>
				<P><BR><BR>
				</P>
			</TD>
		</TR>
		<TR>
			<TD>
				<P>The ASCOM Initiative consists of a group of astronomy software
				developers and instrument vendors whose goals are to promote the
				driver/client model and scripting automation. 
				</P>
				<P>See the <A HREF="https://ascom-standards.org/" TARGET="browser">ASCOM
				web site</A> for more information. Please participate in the
				<A HREF="https://ascomtalk.groups.io/g/Help/topics" TARGET="browser">ASCOM-Talk
				Groups.IO forum</A>. 
				</P>
			</TD>
		</TR>
	</TABLE>
</DIV>
<P><BR><BR>
</P><P>
<BR><BR>
</P>
	
	
	
</BODY>
</HTML>
