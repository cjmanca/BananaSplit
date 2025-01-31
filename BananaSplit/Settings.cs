﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace BananaSplit
{
    public enum ProcessingType
    {
        [Display(Name = "Matroska Chapters")]
        MatroskaChapters,
        [Display(Name = "Split and Encode")]
        SplitAndEncode,
        [Display(Name = "MKVToolNix Split")]
        MKVToolNixSplit
    }

    public class Settings
    {
        public double BlackFrameDuration { get; set; }
        public double BlackFrameThreshold { get; set; }
        public double BlackFramePixelThreshold { get; set; }
        public string FFMPEGArguments { get; set; }
        public ProcessingType ProcessType { get; set; }
        public bool ShowLog { get; set; }
        public bool DeleteOriginal { get; set; }
        public double ReferenceFrameOffset { get; set; }

        public Settings()
        {
            BlackFrameDuration = 0.04;
            BlackFrameThreshold = 0.98;
            BlackFramePixelThreshold = 0.15;
            FFMPEGArguments = "-i \"{source}\" -ss {start} -t {duration} -c:v libx264 -crf 18 -preset slow -c:a copy -map 0 \"{destination}\"";
            ProcessType = ProcessingType.MKVToolNixSplit;
            ShowLog = false;
            DeleteOriginal = false;
            ReferenceFrameOffset = 1;
        }

        public void Load()
        {
            Settings settings;

            if (File.Exists("Settings.json"))
            {
                var json = File.ReadAllText("Settings.json");

                try
                {
                    settings = JsonConvert.DeserializeObject<Settings>(json);
                }
                catch
                {
                    settings = new Settings();
                    settings.Save();
                }
            }
            else
            {
                settings = new Settings();
                settings.Save();
            }

            BlackFrameDuration = settings.BlackFrameDuration;
            BlackFrameThreshold = settings.BlackFrameThreshold;
            BlackFramePixelThreshold = settings.BlackFramePixelThreshold;
            FFMPEGArguments = settings.FFMPEGArguments;
            ProcessType = settings.ProcessType;
            ShowLog = settings.ShowLog;
            DeleteOriginal = settings.DeleteOriginal;
            ReferenceFrameOffset = settings.ReferenceFrameOffset;
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText("Settings.json", json);
        }
    }
}
