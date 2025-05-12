# 🎟️ Nilah's Raffle Plugin

## Want to use this addon? Fork it, build it, allow dev plugins for dalamud and point to the dll. This is no longer being maintained. Screenshot is out of date, but the addon is full featured.

<div align="center">
<img src="raffler.png" alt="Raffler Logo" width="512" height="512">
</div>
✨ A lightweight Dalamud plugin for Final Fantasy XIV that allows players to run in-game raffles using gil as currency. Great for venue events, giveaways, or community nights!

---

## ✨ Current Features

✅ **Toggleable Raffle System**  
Toggle ui with `/raffler`

✅ **Bonus Tickets for Early Entries**  
Configurable "BOGO" (Buy One, Get One) bonus tickets for the first players who enter.

✅ **Chat Trigger Detection**  
Watch for trigger words (like "raffle", "ticket", "join") in chat to automatically open the raffle UI.

✅ **Persistent Entry Saving**  
Raffle entries are saved locally (`raffle_entries.json`) to survive crashes and resume your session safely.

✅ **Sequential Ticket Numbers**  
Every ticket is tracked with a clean, unique number to avoid confusion.

✅ **Dynamic Ticket List Views**  
Choose between a simplified or detailed view of all current entries.

✅ **BOGO Ticket Locking**  
Bonus ticket offers lock after the first bonus is awarded to maintain fairness.

✅ **Intuitive, Lightweight UI**  
Seamless windows to configure raffles, review entries, and draw winners.

✅ **Command Handler and Help Message**  
`/raffler` commands are fully integrated with `/xlhelp` and Dalamud command systems.

✅ **Logging and Debugging**  
Key actions and errors are cleanly logged via Dalamud's plugin log viewer (`/xllog`).

✅ **Name Prefill via Targeting**  
Autofill raffle entries using your current in-game target's name.

---


🏆 **Starting Pot Configuration**  
Set an initial prize pool (gil) for the raffle when it begins.


💬 **Session History Saving**  
Save and review past raffle sessions easily for transparency or event history.

🔢 **Metrics Display**  
Show real-time stats like **Tickets Sold** and **Gil Collected** at the bottom of the raffle window.

⚠️ **Confirmation Prompts**  
Add confirmations before clearing active raffles or deleting data.

---

## 📜 Commands

```plaintext
/raffler        - Open the main UI window
```
---
## 📸 Screenshots
![Preview](https://github.com/user-attachments/assets/dbf6c181-7d0c-4b82-9452-98989e0a252b)


---



