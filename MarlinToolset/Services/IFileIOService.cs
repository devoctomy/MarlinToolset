﻿namespace MarlinToolset.Services
{
    public interface IFileIOService
    {
        string ReadAllText(string path);
        void WriteAllText(
            string path,
            string contents);
    }
}