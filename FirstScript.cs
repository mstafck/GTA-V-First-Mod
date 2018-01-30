using GTA;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

public class FirstScript : Script
{
    int max_companions = 10;
    List<Ped> group_members = new List<Ped>();
 

    public FirstScript()
    {
        Tick += OnTick;
        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;

        Interval = 10;
    }

    void OnTick(object sender, EventArgs e)
    {
        Player player = Game.Player;

        if(player.IsDead && group_members.Count > 0)
        {
            for(int i =0; i < group_members.Count; i++)
            {
                group_members[i].Kill();
                group_members.RemoveAt(i);
            }
        }

        for (int i = 0; i < group_members.Count; i++)
        {
            if(group_members[i].IsDead)
            {
                group_members.RemoveAt(i);
            }
        }
    }
    void OnKeyDown(object sender, KeyEventArgs e)
    {

    }
    void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.H && group_members.Count < max_companions)
        {
            Ped player = Game.Player.Character;
            GTA.Math.Vector3 spawnLoc = player.Position + (player.ForwardVector * 5);

            List<string> model_names = new List<string>();
            model_names.Add("a_c_husky");
            model_names.Add("a_c_retriever");
            model_names.Add("a_c_rottweiler");
            model_names.Add("a_c_shepherd");

            Random rnd = new Random();

            Ped companion = GTA.World.CreatePed(model_names[rnd.Next(0, model_names.Count - 1)], spawnLoc);
            group_members.Add(companion);

            int player_group = GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_PED_GROUP_INDEX, player.Handle);
            GTA.Native.Function.Call(GTA.Native.Hash.SET_PED_AS_GROUP_MEMBER, companion.Handle, player_group);

            companion.Task.ClearAllImmediately();
            GTA.Native.Function.Call(GTA.Native.Hash.TASK_COMBAT_HATED_TARGETS_IN_AREA, companion, 50000, 0);
            GTA.Native.Function.Call(GTA.Native.Hash.SET_PED_KEEP_TASK, companion, true);

        }

        if(e.KeyCode == Keys.J)
        {
            group_members[group_members.Count - 1].Kill();
            group_members.RemoveAt(group_members.Count - 1);
        }
    }
}