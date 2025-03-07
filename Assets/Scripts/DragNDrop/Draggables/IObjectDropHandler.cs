﻿namespace DragNDrop.Draggables
{
    public interface IObjectDropHandler
    {
        void Drop(DraggableObject draggable);

        void CancelDrop(DraggableObject draggable);
    }
}