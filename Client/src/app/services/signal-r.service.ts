import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { signalRApi } from '../app.config';
import { MessageModel } from '../models/message.model';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection: signalR.HubConnection;

  public startConnection = (chatId: number) => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl(signalRApi)
                            .build();
    return this.hubConnection
    .start()
    .then(()=> {
      console.log("Bağlantı sağlandı!");
            
      return this.hubConnection.invoke("JoinGroup", chatId);
    })
    .catch(err=> {
      console.log(err);
      throw err;
    });    
  }

  public JoinGroup = (chatId: number) => {
    this.hubConnection.invoke("JoinGroup", chatId)
    .catch(err => console.log(err));
  }

  public leaveGroup = (chatId: number) => {
    this.hubConnection.invoke("LeaveGroup", chatId)
    .catch(err=> console.log(err));
  }

  public getMessage = (callBack: (message: MessageModel)=> void) => {
    this.hubConnection.on("ReceiveMessage", (res)=> {
      callBack(res);
    });
  }
}
