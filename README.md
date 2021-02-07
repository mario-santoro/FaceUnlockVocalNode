# RecognitionNote

<img align="center" height="204" src="https://github.com/mario-santoro/FaceUnlockVocalNote/blob/master/immagini/icona cloud.png" >

Questa è un app Android sviluppata per l'esame di laurea magistrale informatica curriculum Cloud Computing dell'Università degli Studi di Salerno.
Ha come scopo quello di utilizzare i servizi offerti da Microsoft Azure. 

## Descrizione
L’utente si ritrova inizialmente in una pagina dove deve decidere tra registrazione e login. Essendo il primo accesso decide di registrarsi, inserendo un username (univoco) una password e una foto istantanea che servirà per il suo riconoscimento facciale per i prossimi accessi. Completata la procedura si ritroverà nella HomePage dell’app.
Effettuando successivamente il login può scegliere se effettuare l’accesso in modo classico (username e password) oppure con username e <b>riconoscimento facciale</b>. Utilizzando quest’ultimo all’accesso verrà fatto <b>l'emotion recognition</b> della foto (8 tipi diversi di emozioni percepite), in base a quest'ultima verrà visualizzato un tema differente dell'app (ogni colore messo ad hoc per mantenere o cambiare l'umore dell'utente in maniera positiva) e un messaggio di benvenuto. 
Il servizio in sé permette all’utente di tener traccia di note testuali (inserimento, cancellazione e modifica) con la possibilità di scattare una foto a del testo o un immagine contenente testo e poi il servizio cognitivo <b>riconosce il testo</b> in essa contenuta e in maniera automatica inserisce il testo riconosciuto nel contenuto della nota, pronta per essere salvata o modificata. Infine con il <b>Sentiment Analysis</b> del testo viene inserita un emoji a seconda del sentimento riscontrato nel testo.

## Architettura
<img align="center" height="250" src="https://github.com/mario-santoro/RecognitionNote/blob/master/immagini/architettura.jpg?raw=true" >

## Componenti di Microsoft Azure utilizzate 
 <ul>
	<li>Database SQL: per memorizzare in maniera persistente i dati anagrafici e di accesso dell’utente e le note testuali.</li>
	<li>l'API Visione artificiale in Servizi cognitivi di Azure: 
	<ul>
		<li>Face - Analizza i visi umani in un'immagine riconoscimento facciale e emotion recognition.</li>
		<li>Vision: OCR e Lettura - estrazione di testo (riconoscimento ottico dei caratteri) stampato o scritto a mano da immagini e creare le note testuali.</li> 
		<li>Text Analytics: Sentiment Analysis – un’analisi dei sentimenti e delle opinioni espresse nei testi.</li>
	</ul>
</ul>

## Installazione di pacchetti NuGet
- System.Data.SqlClient
- Cognitive services: Install-Package Microsoft.Azure.CognitiveServices.Vision.Face -Version 2.6.0-preview.1

## Tabelle Database
<b>create table utente</b>(
  <br> username Nvarchar(20) primary key,
 <br> passw Nvarchar(100) not null,
 <br>  personID varchar(40)
<br>)
<br>
<b>create table nota</b>(
 <br>   id_nota int Primary key,
  <br>  titolo Nvarchar(20) not null,
    <br>data_nota Nvarchar(50) not null,
    <br>contenuto Nvarchar(4000) not null,
    <br>username Nvarchar(20) NOT NULL,
    <br> FOREIGN KEY (username) REFERENCES utente(username)
     <br>on Update cascade
     <br>on Delete cascade
<br>)

##About Us
<img align="center" height="204" src="https://github.com/mario-santoro/FaceUnlockVocalNote/blob/master/immagini/aboutUs.png" >
