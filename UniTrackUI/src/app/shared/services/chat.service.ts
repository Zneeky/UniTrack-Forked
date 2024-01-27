import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection!: signalR.HubConnection;

  constructor() 
    {
    // Initialize the hubConnection property
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/api/hubs/chatHub', { withCredentials: true }) // Update with the URL to your SignalR hub
      .build();
  }

  public startConnection = (): Promise<void> => {
    return this.hubConnection
      .start()
      .then(() => console.log('Connection Started.'))
      .catch(err => console.error('Error while starting connection: ' + err));
  }

  public sendMessageToUser = (receiverUserId: string, message: string): Promise<void> => {
    // Invoke the SendMessageToUser method defined in the ChatHub on the server
    return this.hubConnection.invoke('SendMessageToUser', receiverUserId, message)
      .catch(err => console.error(err));
  }

  public registerOnMessageReceived = (callback: (senderUserId: string, message: string) => void): void => {
    // Register a handler that's invoked when the hub calls the ReceiveMessage method
    this.hubConnection.on('ReceiveMessage', callback);
  }

  // Add any additional methods you might need, like disconnecting, etc.
}
