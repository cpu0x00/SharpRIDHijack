# SharpRIDHijack
a little tool to play with RIDs for sneaky persistence

```
.\SharpRIDHijack.exe -h

--user, -u                   username to use
--password, -p                 password to use [if present without --enable a new user will be created otherwise it will enable a user and change its password]
--enable                       enables an existing disabled user and hijackes its rid
--elevate                      if present will attempt SYSTEM impersonation (use if you are not SYSTEM already)
```

- Make any user a local admin without adding them to any group
- Bypasses the `LocalAccountTokenFilterPolicy` as the user's RID in the registry will be 500 
