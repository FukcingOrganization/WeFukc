# We Fukc
Hi there, my name is Bora Ozenbirkan and I'm the creator of this project. Initially, I started to make this blockchain game as a **fun experiment** but after a while, the idea evolved into something meaningful and exciting. Then I decided to change its name ****to Stick Fight, instead of We Fukc.**** Because it was going to be a serious project. 

**Here some useful links:**
- **[Demo](https://fukcingorganization.github.io/Demo/)**: You can connect your wallet and play the on web!
- **[Demo Video](https://youtu.be/ziFFLVKjOFQ)**: If you don't own a wallet, you just watch the gameplay!
- **[Contracts](https://github.com/FukcingOrganization/Contracts-Private/tree/main)**: 11 Smart contract with +6200 lines of code that makes the game fully on-chain.
- **[GitBook](https://bora-oezenbirkan.gitbook.io/we-fukc-white-paper/)**: You can learn almost every detail of the project and see the calculations about different subject.

## Gameplay
Initially, the game was about fighting with enemies and completing levels. To complete the level, you need to **drop the key** of the exit elevator from the enemies. So, you can't just run and finish the level, you need to kick all asses until you drop the key from them!
<p align="center">
<img src="https://github.com/FukcingOrganization/.github/blob/main/profile/Images/WeFukc_Gameplay.gif" alt="Japanese Warrior" width="500"/>


The interesting part begins when you complete the level. You are encountering the **final boss** of that level. But, instead of fighting with the Boss, you make love with the Boss!

> **"Don't fight, make love"**

Not appropriate for underage teenagers but **I wasn't really considering releasing the game as it was.** The initial idea was minting game items (sword, axe, pistol, machine gun, etc.) with a Web3 wallet by burning some amount of game token.

### The First Evolution
I started to add new ideas to the game. First, I wanted to give a name and meaning to every boss. So, the fight was against these names or terms. The name of the first level was **"Me"** because a person starts to question and face himself/herself at first! Then I named the second boss **"War"** and the third boss **"Inflation"**. Things we suffer from recently.

Then I thought, it would be cool if the players would choose and change the boss every week for each level. **But how do they choose?**

I decided to make bosses as NFT characters. Players can **mint and create a boss, give it a name, and make it a candidate** for a level for the next week. Then people would **fund** these bosses with the game token **($FUKC)**. The most funded boss would be elected and get fukced by players in that week.

The funds of the winner boss would be distributed to the players of that level and the funds of the loser bosses would **burn** and create a **deflation** in tokenomics.
<p align="center">
<img src="https://github.com/FukcingOrganization/.github/blob/main/profile/Images/WeFukc_Burn.png" alt="Japanese Warrior" width="400"/>

Creating a boss, naming it, pointing it as a candidate, and electing a boss for each level every week. I thought it would be fun!

### The Second Evolution
Then I started to think about its social meaning. How people will decide who to choose, why. **Discussions** about the situation (war, inflation, economy, crypto, etc.) **were the key point** of the mechanism.

I thought if we will discuss it, we should **meet at pubs, cafes, squares, and parks, and discuss it face to face, get in touch with each other.** It can be a reason for going outside, meeting with people, knowing new faces, hearing new ideas, **learning new things, teach new things!**

It would be awesome! We could bring new people who don't know anything about crypto and we could teach them. They can give us feedback as an outsider to the industry. All of these interactions would be great!

Then I thought we should have local **Clans** to enhance these meetings and feel as a whole and be a part of something. And these clans should have **Lords** who create these clans.

We had to create a **DAO** to run this organization in a **decentralized manner** and I wrote a DAO contract. DAO tokens **can not be transferable, they only can be earned** by gameplay! So, players run the DAO, not the VCs. And I wrote an **Executors Contract** to assign officers to executors on-chain activities like proposing mint cost changes, etc. **DAO can fire or hire the Executors with voting.**
<p align="center">
<img src="https://github.com/FukcingOrganization/.github/blob/main/profile/Images/WeFukc_UI.gif" alt="Japanese Warrior" width="500"/>
<p align="center" style="font-size:14px"><em>Profile UI</em>

### The Final Image
In the final form of the game, we would distribute the game reward tokens to the backers who fund the bosses for each level, winners' funds would go to the players, and losers' would burn.

And there would be clans who meet in public spaces and discuss the hot topics of the day, and decide who or what to choose as a boss in the next week. And Lord who create these clans.

Of course, at this point, I have decided to change the name **from We Fukc to Stick Fight.** Because it was a serious and social game from now on. And at the end of the level, you would fight against a final boss, not make love with it.
<p align="center">
<img src="https://github.com/FukcingOrganization/.github/blob/main/profile/Images/WeFukc_Transaction.gif" alt="Japanese Warrior" width="500"/>
<p align="center" style="font-size:14px"><em>In-game Transaction with Wallet</em>

## Key Features
- Fully **on-chain game** mechanics
- **Advanced and sustainable** tokenomcics and mechanics
- DAO: **Untransferable tokens** only can be earned with gameplay!
- **Executors**: Officers to execute some tasks assigned by DAO
- Renting: Creating **passive income** by renting Lords!
- No download is needed with **WebGL gaming**
- **Social game** environment, regular **real-world meetings**

## Challenges and Implementation
✅ **Stick Man Animations:** The first challenge for me was to create a stick character to fight. The hard part was the animations because I wanted to make them feel really stickman. So, I didn't use image animations. I made the characters as rigged dolls. Worked hard on their animations and the physics of their death. When they die near an edge and fall from the edge slowly, you feel the realistic stick fight.

✅ **Advanced Smart Contracts:** The second and biggest challenge was to **MAKE THE GAME FULLY ON CHAIN!**** It wasn't easy. I wrote **11 smart contracts**. Spend months on tests and bug fixes. I was frustrated because when I found a bug in a contract, I update it but with that update, I had to start the giant simulation test which includes 11 smart contracts, all over again, again, and again. It wasn't easy, but I did it. **11 smart contracts, 6200 lines of code just for these smart contracts!**

✅❗️ **Blockchain Interactions:** The next big challenge was to implement the smart contract into the game. It is easily done with web applications but it was so **hard to do it with a WebGL Unity game**. Because current service providers like **Moralis and ChainSafe were expensive and not even ready to use** sufficiently. At the time Moralis was changing its whole system and you were ending up with deprecated documents. It wasn't reliable. So, I found **Nethereum**. It lacks much documentation but provides example projects. I had help from the community and the creator of Nethereum, **[Juan Blanco](https://github.com/juanfranblanco)**. He helped me a lot. **I did manage to read data from the blockchain and write data into the blockchain via making transactions with Metamask wallet**. It was so hard to do at first, but I did manage to do that.❗️**But I failed to get the results of the transactions to update the UI accordingly**. It was problematic with Nethereum in Unity. We discussed this with Juan on this subject and he released an update for it but, at the time of the update, **I already gave up** and working on my new project called **Zk Chickens**. I failed to read transaction results and couldn't update the UI accordingly.

✅ **UI/UX:** Another challenge was the UI and UX. Unity is a game engine and it was really hard to create complex interfaces within the game. It is easy to do in Web applications or mobile applications but was hard to do such a complex interface in Unity.

## Results
The idea started as a **fun experiment** and has turned into a **social and serious game project**. But as I mentioned above (Blockchain interaction challenge), I failed to complete the Blockchain interactions in the game with WebGL.

The difficulty that I had technical problems summed up with the realization of the difficulty to market this game, letting people go outside and meet at cafes, discussing hot topics, etc.

It seemed obvious, even though I could manage to finish and launch the game, **it would fail**. And it was the time of the Arbitrum airdrop and everyone was looking for something to use on Zk Sync. So, I decided to use this opportunity and started to make a simple game called Zk Chickens. You should definitely check it out. **I finished it but I even couldn't market it**. So, giving up on Stick Fight (Formerly We Fukc) was the right decision to do.

Thank you for your interest.
