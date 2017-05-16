
using System.Collections.Generic;

namespace ASD
{

public interface IList : IEnumerable<int>
    {
    // Jeœli element v jest na liœcie to zwraca true
    // Jeœli elementu v nie ma na liœcie to zwraca false
    bool Search(int v);

    // Jeœli element v jest na liœcie to zwraca false (elementu nie dodaje)
    // Jeœli elementu v nie ma na liœcie to dodaje go do listy i zwraca true
    bool Add(int v);

    // Jeœli element v jest na liœcie to usuwa go z listy i zwraca true
    // Jeœli elementu v nie ma na liœcie to zwraca false
    bool Remove(int v);
    }

//
// dopisaæ klasê opisuj¹c¹ element listy
//
public class Elem
    {
        public Elem next;
        public int key;

        public Elem(int k, Elem n = null)
        {
            key = k;
            next = n;
        }
    }

// Zwyk³a lista (nie samoorganizuj¹ca siê)
public class SimpleList : IList
    {

        // dodaæ niezbêdne pola
    public Elem head, tail;
    // Lista siê nie zmienia
    public bool Search(int v)
        {
            if (head == null)
                return false;
            Elem p = head;
            while (p != null)
            {
                if (p.key == v)
                    return true;
                p = p.next;
            }
            return false;  // zmieniæ
        }

    // Element jest dodawany na koniec listy
    public bool Add(int v)
        {
            if (head == null)
            {
                head = tail = new Elem(v);
                return true;
            }
            Elem p = head;
            while (p != null)
            {
                if (p.key == v)
                    return false;
                p = p.next;
            }
            tail.next = new Elem(v);
            tail = tail.next;
            return true;  // zmieniæ
        }

    // Pozosta³e elementy nie zmieniaj¹ kolejnoœci
    public bool Remove(int v)
        {
            if (head == null)
                return false;
            Elem pp = null, p = head;
            while (p != null)
            {
                if (p.key == v)
                    break;
                pp = p;
                p = p.next;
            }
            if (p == null)
                return false;
            if (pp == null)
                head = p.next;
            else if (p.next == null)
            {
                pp.next = null;
                tail = pp;
            }
            else
                pp.next = p.next;
            return true;  // zmieniæ
        }

    // Wymagane przez interfejs IEnumerable<int>
    public IEnumerator<int> GetEnumerator()
        {
            // nie wolno modyfikowaæ kolekcji
            Elem p = head;
            while (p != null)
            {
                yield return p.key;
                p = p.next;
            }
        }

    // Wymagane przez interfejs IEnumerable<int> - nie zmmieniaæ (jest gotowe!)
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
        return this.GetEnumerator();
        }

    } // class SimpleList


// Lista z przesnoszeniem elementu, do którego by³ dostêp na pocz¹tek
public class MoveToFrontList : IList
    {

        // dodaæ niezbêdne pola
        public Elem head;
    // Znaleziony element jest przenoszony na pocz¹tek
    public bool Search(int v)
        {
            if (head == null)
                return false;
            Elem pp = null, p = head;
            while (p != null)
            {
                if (p.key == v)
                {
                    if (p != head)
                    {
                        pp.next = p.next;
                        p.next = head;
                        head = p;
                    }
                    return true;
                }
                pp = p;
                p = p.next;
            }
            return false;  // zmieniæ
        }

    // Element jest dodawany na pocz¹tku, a jeœli ju¿ by³ na liœcie to jest przenoszony na pocz¹tek
    public bool Add(int v)
        {
            if (Search(v))
                return false;
            head = new Elem(v, head);
            return true;
        }

    // Pozosta³e elementy nie zmieniaj¹ kolejnoœci
    public bool Remove(int v)
        {
            if (head == null)
                return false;
            Elem pp = null, p = head;
            while (p != null)
            {
                if (p.key == v)
                {
                    if (p == head)
                        head = p.next;
                    else
                        pp.next = p.next;
                    return true;
                }
                pp = p;
                p = p.next;
            }
            return false;  // zmieniæ
        }

    // Wymagane przez interfejs IEnumerable<int>
    public IEnumerator<int> GetEnumerator()
        {
            // nie wolno modyfikowaæ kolekcji
            Elem p = head;
            while (p != null)
            {
                yield return p.key;
                p = p.next;
            }
        }

    // Wymagane przez interfejs IEnumerable<int> - nie zmmieniaæ (jest gotowe!)
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
        return this.GetEnumerator();
        }

    } // class MoveToFrontList


} // namespace ASD
