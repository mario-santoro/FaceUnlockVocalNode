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

## Usage
<ul>
	<li>Database SQL</li>
	<li>Servizi cognitivi: 
	<ul>
		<li>Face: Una volta recatosi sul protale Azure di Microsoft l'utente dovrà creare la riorsa Face andando a settare i seguenti parametri</li>
		<ul>
			<li>Gruppo di risorse (Cerarne uno nuovo nel caso non vi è presente già uno precedentemente creato)</li>
			<li>Settare la regione del server a quale fare riferimento</li>
			<li>Settare il nome della risorsa che si sta creando</li>
			<li>Andare a selezionare la fatturazizone del servizio (F0 darà risorse gratis fino a 5k chiamate al minuto e 30k chiamate al mese)</li>
		</ul>
		<li>Vision: OCR e Lettura</li> 
		<li>Text Analytics: Sentiment Analysis</li>
	</ul>
</ul>

## Installazione di pacchetti NuGet
Per poter andare a utilizzare librerie che si interfacciano con gli strumenti messi a disposizione da Azure, da Visual Studio bisogna, dopo aver creato un progetto App Android (Xamarin), bisogna cliccare con il tasto destro sul nome del progetto, presente nel pannello esplora risorse sulla destra dell'interfaccia, e selezionare l'opzione pacchetti NuGet, da qui ci si aprirà una schermata dalla quale potremmo vedere i pacchetti già installati, quelli da aggiornare e installarne di nuovi. Cliccando sul tab "installa" avremo avanti una schermata che ci proporrà tutti i pacchetti messi a disposizione, e tramtite una ricerca possiamo andare a filtrare i pacchetti proposti. Quelli da ricercare e installare sono i seguenti: 
<ul>
	<li>System.Data.SqlClient</li>
	<li>Cognitive services</li>
</ul>
Per il pacchetti "Cognitive services" bisogna prima andare sulla console di gestione di pacchetti NuGet di progetto, dalla quale possiamo accedere tramite il menù a tendina su Visual studio nominato "Strumenti" -> "Gestione pacchetti NuGet" -> "Console", e inserire il seguente codice 
```console
foo@bar:~$ Install-Package Microsoft.Azure.CognitiveServices.Vision.Face -Version 2.6.0-preview.1

```

## About Us
<img align="center" height="300" src="https://github.com/mario-santoro/FaceUnlockVocalNote/blob/master/immagini/aboutUs.png" >
