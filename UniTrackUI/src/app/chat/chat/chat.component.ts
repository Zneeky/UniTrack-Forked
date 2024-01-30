import { Component, OnInit } from '@angular/core';
import { tap } from 'rxjs';
import { LocalStorageKeys } from 'src/app/shared/enums/local-storage-keys.enum';
import { MessageHistoryResult } from 'src/app/shared/models/message-hisotry';
import { MessageResult } from 'src/app/shared/models/message-result';
import { UserResult } from 'src/app/shared/models/user-result';
import { ChatRequestsService } from 'src/app/shared/services/chat.requests.service';
import { ChatService } from 'src/app/shared/services/chat.service';


interface ChatEntry {
  personName: string;
  messages: Message[];
  receiverUserId: string;
}

interface Message {
  text: string;
  sentBy: 'me' | 'friend';
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
})
export class ChatComponent implements OnInit {

  searchText: string = '';
  contactsData: UserResult[] = [];
  message: string = '';
  chatsHistory: MessageHistoryResult[] = [];
  selected: any;
  color: any;
  isSelectedChat: boolean = false;

  constructor(
    // private teachersService: TeachersService,
    public chatService: ChatService,

    public chatRequestsService: ChatRequestsService,
      ) 
      
  {}
  selectedChatIndex: number | null = null;
  showSettings: boolean = false;
  centralChatBackgroundImage: any = localStorage.getItem(LocalStorageKeys.BACKGROUND_ID);
  
  ngOnInit(): void {
    
    
    this.chatRequestsService
      .getContacts()
      .pipe(tap((contacts) => (this.contactsData = contacts)))
      .subscribe();

      this.chatService.startConnection().then(() => {
        this.chatService.registerOnMessageReceived((senderUserId, message) => {
          // Handle the received message here (update your chat UI)
          const messageObject:Message ={
            text:message,
            sentBy:'friend'
          }
          this.getMessageHistory();
          this.selectedChat.messages.push(messageObject)
          console.log(`Message from ${senderUserId}: ${message}`);
    })
    
    
  })
  
  this.chatRequestsService.getMessageHistory() 
  .pipe(tap((chat) => (this.chatsHistory = chat)))
  .subscribe();

  
  
}

  getMessageHistory() {
    this.chatRequestsService
      .getMessageHistory()
      .pipe(tap((chat) => (this.chatsHistory = chat)))
      .subscribe();
  }

    filteredChatHistory(): MessageHistoryResult[] {
      return this.chatsHistory.filter(chat =>
        chat.firstName.toLowerCase().includes(this.searchText.toLowerCase())
      );
    }
    
  selectedChat: ChatEntry = {
    personName: '',
    messages: [] as Message[],
    receiverUserId: '',
  };

  newMessage: string = '';
  selectChat(personName: string, userId: string , index : number): void {
    this.selectedChat = {
      personName: personName,
      messages: [],
      receiverUserId: userId,
    };
    this.selectedChatIndex = index;    
   
    this.isSelectedChat = true;
    this.chatRequestsService.getMessagesInChat(userId).subscribe(
      (messageResults: MessageResult[]) => {
        this.selectedChat.messages = messageResults.map((msgResult) => {
          return {
            text: msgResult.content,
            sentBy: msgResult.senderId === userId ? 'friend' : 'me',
          };
        });
      },
      (error) => {
        console.error('Error loading messages:', error);
      }
    );
  }
  toggleSettings(): void {
    this.showSettings = !this.showSettings;
  }
  changeCentralChatBackground(color: string): void {
    this.centralChatBackgroundImage = color;
  }

  changeBackgroundToImage1(): void {
    const url ='url("https://images.assetsdelivery.com/compings_v2/mariabo2015/mariabo20152107/mariabo2015210700030.jpg")';
    localStorage.setItem(LocalStorageKeys.BACKGROUND_ID,url)
    this.centralChatBackgroundImage = localStorage.getItem(LocalStorageKeys.BACKGROUND_ID);
  }

  changeBackgroundToImage2(): void {
    const url = 'url("https://images.assetsdelivery.com/compings_v2/mariabo2015/mariabo20152107/mariabo2015210700027.jpg")';
    localStorage.setItem(LocalStorageKeys.BACKGROUND_ID,url)
    this.centralChatBackgroundImage = localStorage.getItem(LocalStorageKeys.BACKGROUND_ID);
  }

  changeBackgroundToImage3(): void {
    const url = 'url("https://media.istockphoto.com/id/1256183538/vector/hand-drawn-seamless-pattern-of-creatives-themes-doodle-element-set-vector-illustration.jpg?s=612x612&w=0&k=20&c=gkbWqXQqegrq1pwwY8H7k-QPavmBAnQlbZOhbwM7WV4=")';
    localStorage.setItem(LocalStorageKeys.BACKGROUND_ID,url)
    this.centralChatBackgroundImage = localStorage.getItem(LocalStorageKeys.BACKGROUND_ID);
  }
  changeBackgroundToImage4(): void {
    const url = 'url(" https://img.freepik.com/premium-vector/back-school-doodle-sketch-cartoon-seamless-pattern_221062-914.jpg")';
    localStorage.setItem(LocalStorageKeys.BACKGROUND_ID,url)
    this.centralChatBackgroundImage = localStorage.getItem(LocalStorageKeys.BACKGROUND_ID);

  }

  changeBackgroundToImage5(): void {
    const url = 'url(https://images.rawpixel.com/image_800/czNmcy1wcml2YXRlL3Jhd3BpeGVsX2ltYWdlcy93ZWJzaXRlX2NvbnRlbnQvbHIvdjk5Ni0wMDlfMS1rcm9pcjRkay5qcGc.jpg)';
    localStorage.setItem(LocalStorageKeys.BACKGROUND_ID,url)
    this.centralChatBackgroundImage = localStorage.getItem(LocalStorageKeys.BACKGROUND_ID);

  }
  changeBackgroundToImage6(): void {
    const url = 'url(https://www.bhphotovideo.com/images/images500x500/Savage_27_12_107_x_12yds_Background_1441903824_45483.jpg)';
    localStorage.setItem(LocalStorageKeys.BACKGROUND_ID,url)
    this.centralChatBackgroundImage = localStorage.getItem(LocalStorageKeys.BACKGROUND_ID);

  }



  

  

  sendMessage(): void {
    if (!this.newMessage.trim()) return;

    this.chatService
      .sendMessageToUser(this.selectedChat.receiverUserId, this.newMessage)
      .then(() => {
        const newMessage: Message = {
          text: this.newMessage,
          sentBy: 'me',
        };
        this.selectedChat.messages.push(newMessage);

        this.newMessage = '';

        console.log('Message sent');
      })
      .catch((error) => {
        console.error('Error sending message:', error);
      });
    this.getMessageHistory();

    
    
  }

 
}
